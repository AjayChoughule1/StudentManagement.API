using StudentManagement.API.Domain.DTOs;
using StudentManagement.API.Domain.Entities;
using StudentManagement.API.Domain.Interfaces;

namespace StudentManagement.API.Domain.Services
{
    public class AttendanceService
    {
        private readonly IUnitOfWork _uow;

        public AttendanceService(IUnitOfWork uow) => _uow = uow;

        // Get attendance for a class on a specific date
        public async Task<IEnumerable<AttendanceDto>> GetByClassAndDateAsync(int classRoomId, DateTime date)
        {
            var records = await _uow.Attendances.GetByClassAndDateAsync(classRoomId, date.Date);
            return records.Select(MapToDto);
        }

        // Get full attendance history of a single student
        public async Task<IEnumerable<AttendanceDto>> GetByStudentAsync(int studentId, DateTime? from, DateTime? to)
        {
            var records = await _uow.Attendances.GetByStudentAsync(studentId, from, to);
            return records.Select(MapToDto);
        }

        // Bulk mark attendance for an entire class for a given day
        public async Task BulkMarkAsync(BulkMarkAttendanceDto dto)
        {
            _ = await _uow.ClassRooms.GetByIdAsync(dto.ClassRoomId)
                ?? throw new KeyNotFoundException($"ClassRoom {dto.ClassRoomId} not found.");
            _ = await _uow.Teachers.GetByIdAsync(dto.MarkedByTeacherId)
                ?? throw new KeyNotFoundException($"Teacher {dto.MarkedByTeacherId} not found.");

            var date = dto.Date.Date;
            var attendances = dto.Attendances.Select(a => new Attendance
            {
                StudentId = a.StudentId,
                ClassRoomId = dto.ClassRoomId,
                MarkedByTeacherId = dto.MarkedByTeacherId,
                Date = date,
                Status = a.Status,
                Remarks = a.Remarks,
                MarkedAt = DateTime.UtcNow
            });

            await _uow.Attendances.BulkInsertOrUpdateAsync(attendances);
            await _uow.SaveChangesAsync();
        }

        // Monthly report for all students in a class
        public async Task<IEnumerable<AttendanceReportDto>> GetMonthlyReportAsync(int classRoomId, int year, int month)
        {
            var records = await _uow.Attendances.GetByClassAndMonthAsync(classRoomId, year, month);
            var students = await _uow.Students.GetByClassRoomAsync(classRoomId);

            return students.Select(s =>
            {
                var studentRecords = records.Where(r => r.StudentId == s.Id).ToList();
                return new AttendanceReportDto
                {
                    StudentId = s.Id,
                    StudentName = $"{s.FirstName} {s.LastName}",
                    RollNumber = s.RollNumber,
                    TotalDays = studentRecords.Count,
                    PresentDays = studentRecords.Count(r => r.Status == AttendanceStatus.Present),
                    AbsentDays = studentRecords.Count(r => r.Status == AttendanceStatus.Absent),
                    LeaveDays = studentRecords.Count(r => r.Status == AttendanceStatus.Leave),
                    HalfDays = studentRecords.Count(r => r.Status == AttendanceStatus.HalfDay)
                };
            });
        }

        // Dashboard: today's attendance % across all classes
        public async Task<double> GetTodayAttendancePercentageAsync()
        {
            var today = DateTime.UtcNow.Date;
            var classes = await _uow.ClassRooms.GetActiveClassRoomsAsync();
            int total = 0, present = 0;
            foreach (var c in classes)
            {
                var records = await _uow.Attendances.GetByClassAndDateAsync(c.Id, today);
                total += records.Count();
                present += records.Count(r => r.Status == AttendanceStatus.Present);
            }
            return total == 0 ? 0 : Math.Round((double)present / total * 100, 2);
        }

        private static AttendanceDto MapToDto(Attendance a) => new()
        {
            Id = a.Id,
            Date = a.Date,
            Status = a.Status.ToString(),
            Remarks = a.Remarks,
            StudentId = a.StudentId,
            StudentName = a.Student is null ? string.Empty : $"{a.Student.FirstName} {a.Student.LastName}",
            RollNumber = a.Student?.RollNumber ?? string.Empty,
            ClassRoomId = a.ClassRoomId,
            ClassRoomName = a.ClassRoom is null ? string.Empty : $"{a.ClassRoom.Name} {a.ClassRoom.Section}",
            MarkedByTeacherId = a.MarkedByTeacherId,
            TeacherName = a.MarkedByTeacher is null
                                    ? string.Empty
                                    : $"{a.MarkedByTeacher.FirstName} {a.MarkedByTeacher.LastName}"
        };
    }
}

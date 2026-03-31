using StudentManagement.API.Domain.Entities;

namespace StudentManagement.API.Domain.DTOs
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string RollNumber { get; set; } = string.Empty;
        public int ClassRoomId { get; set; }
        public string ClassRoomName { get; set; } = string.Empty;
        public int MarkedByTeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
    }

    public class MarkAttendanceItemDto
    {
        public int StudentId { get; set; }
        public AttendanceStatus Status { get; set; }
        public string? Remarks { get; set; }
    }

    public class BulkMarkAttendanceDto
    {
        public DateTime Date { get; set; }
        public int ClassRoomId { get; set; }
        public int MarkedByTeacherId { get; set; }
        public List<MarkAttendanceItemDto> Attendances { get; set; } = new();
    }

    public class AttendanceReportDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string RollNumber { get; set; } = string.Empty;
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int LeaveDays { get; set; }
        public int HalfDays { get; set; }
        public double AttendancePercentage => TotalDays == 0 ? 0 : Math.Round((double)PresentDays / TotalDays * 100, 2);
    }

    public class DashboardStatsDto
    {
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalClasses { get; set; }
        public double TodayAttendancePercentage { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Domain.Entities;
using StudentManagement.API.Domain.Interfaces;
using StudentManagement.API.Infrastructure.Data;

namespace StudentManagement.API.Infrastructure.Repository
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(AppDbContext db) : base(db) { }

        public async Task<IEnumerable<Attendance>> GetByClassAndDateAsync(int classRoomId, DateTime date) =>
            await _db.Attendances
                .Include(a => a.Student)
                .Include(a => a.MarkedByTeacher)
                .Where(a => a.ClassRoomId == classRoomId && a.Date.Date == date.Date)
                .ToListAsync();

        public async Task<IEnumerable<Attendance>> GetByStudentAsync(int studentId, DateTime? from = null, DateTime? to = null)
        {
            var query = _db.Attendances
                .Include(a => a.ClassRoom)
                .Include(a => a.MarkedByTeacher)
                .Where(a => a.StudentId == studentId);

            if (from.HasValue) query = query.Where(a => a.Date >= from.Value);
            if (to.HasValue) query = query.Where(a => a.Date <= to.Value);

            return await query.OrderByDescending(a => a.Date).ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByClassAndMonthAsync(int classRoomId, int year, int month) =>
            await _db.Attendances
                .Include(a => a.Student)
                .Where(a => a.ClassRoomId == classRoomId &&
                            a.Date.Year == year &&
                            a.Date.Month == month)
                .ToListAsync();

        public async Task<bool> AttendanceExistsAsync(int studentId, DateTime date) =>
            await _db.Attendances.AnyAsync(a => a.StudentId == studentId && a.Date.Date == date.Date);

        public async Task BulkInsertOrUpdateAsync(IEnumerable<Attendance> attendances)
        {
            foreach (var a in attendances)
            {
                var existing = await _db.Attendances
                    .FirstOrDefaultAsync(x => x.StudentId == a.StudentId && x.Date.Date == a.Date.Date);

                if (existing is null)
                    await _db.Attendances.AddAsync(a);
                else
                {
                    existing.Status = a.Status;
                    existing.Remarks = a.Remarks;
                    existing.MarkedAt = DateTime.UtcNow;
                    existing.MarkedByTeacherId = a.MarkedByTeacherId;
                    _db.Entry(existing).State = EntityState.Modified;
                }
            }
        }
    }
}

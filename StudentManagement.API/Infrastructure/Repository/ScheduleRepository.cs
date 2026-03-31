using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Domain.Entities;
using StudentManagement.API.Domain.Interfaces;
using StudentManagement.API.Infrastructure.Data;

namespace StudentManagement.API.Infrastructure.Repository
{
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(AppDbContext db) : base(db) { }

        public async Task<IEnumerable<Schedule>> GetByTeacherAsync(int teacherId) =>
            await _db.Schedules
                .Include(s => s.ClassRoom)
                .Where(s => s.TeacherId == teacherId && s.IsActive)
                .OrderBy(s => s.DayOfWeek).ThenBy(s => s.StartTime)
                .ToListAsync();

        public async Task<IEnumerable<Schedule>> GetByClassRoomAsync(int classRoomId) =>
            await _db.Schedules
                .Include(s => s.Teacher)
                .Where(s => s.ClassRoomId == classRoomId && s.IsActive)
                .OrderBy(s => s.DayOfWeek).ThenBy(s => s.StartTime)
                .ToListAsync();

        public async Task<bool> HasConflictAsync(int teacherId, DayOfWeek day, TimeSpan start, TimeSpan end, int? excludeId = null) =>
            await _db.Schedules.AnyAsync(s =>
                s.TeacherId == teacherId &&
                s.DayOfWeek == day &&
                s.Id != excludeId &&
                s.IsActive &&
                s.StartTime < end && s.EndTime > start);  // overlap check
    }
}

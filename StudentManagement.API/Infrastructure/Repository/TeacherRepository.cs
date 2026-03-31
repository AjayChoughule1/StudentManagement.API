using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Domain.Entities;
using StudentManagement.API.Domain.Interfaces;
using StudentManagement.API.Infrastructure.Data;

namespace StudentManagement.API.Infrastructure.Repository
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(AppDbContext db) : base(db) { }

        public async Task<Teacher?> GetWithSchedulesAsync(int teacherId) =>
            await _db.Teachers
                .Include(t => t.Schedules).ThenInclude(s => s.ClassRoom)
                .FirstOrDefaultAsync(t => t.Id == teacherId);

        public async Task<IEnumerable<Teacher>> GetActiveTeachersAsync() =>
            await _db.Teachers
                .Include(t => t.Schedules)
                .Where(t => t.IsActive)
                .ToListAsync();

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null) =>
            await _db.Teachers.AnyAsync(t => t.Email == email && t.Id != excludeId);
    }
}

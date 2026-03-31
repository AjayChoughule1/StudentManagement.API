using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Domain.Entities;
using StudentManagement.API.Domain.Interfaces;
using StudentManagement.API.Infrastructure.Data;

namespace StudentManagement.API.Infrastructure.Repository
{
    public class ClassRoomRepository : Repository<ClassRoom>, IClassRoomRepository
    {
        public ClassRoomRepository(AppDbContext db) : base(db) { }

        public async Task<ClassRoom?> GetWithStudentsAsync(int id) =>
            await _db.ClassRooms.Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<ClassRoom?> GetWithSchedulesAsync(int id) =>
            await _db.ClassRooms
                .Include(c => c.Schedules).ThenInclude(s => s.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<ClassRoom>> GetActiveClassRoomsAsync() =>
            await _db.ClassRooms.Where(c => c.IsActive).ToListAsync();
    }
}

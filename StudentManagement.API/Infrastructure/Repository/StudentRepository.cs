using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Domain.Entities;
using StudentManagement.API.Domain.Interfaces;
using StudentManagement.API.Infrastructure.Data;

namespace StudentManagement.API.Infrastructure.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext db) : base(db) { }

        public override async Task<IEnumerable<Student>> GetAllAsync() =>
            await _db.Students.Include(s => s.ClassRoom).ToListAsync();

        public override async Task<Student?> GetByIdAsync(int id) =>
            await _db.Students.Include(s => s.ClassRoom).FirstOrDefaultAsync(s => s.Id == id);

        public async Task<IEnumerable<Student>> GetByClassRoomAsync(int classRoomId) =>
            await _db.Students
                .Include(s => s.ClassRoom)
                .Where(s => s.ClassRoomId == classRoomId && s.IsActive)
                .OrderBy(s => s.RollNumber)
                .ToListAsync();

        public async Task<Student?> GetByRollNumberAsync(string rollNumber) =>
            await _db.Students.Include(s => s.ClassRoom)
                .FirstOrDefaultAsync(s => s.RollNumber == rollNumber);

        public async Task<IEnumerable<Student>> SearchAsync(string term) =>
            await _db.Students.Include(s => s.ClassRoom)
                .Where(s => s.FirstName.Contains(term) ||
                            s.LastName.Contains(term) ||
                            s.RollNumber.Contains(term) ||
                            s.Email.Contains(term))
                .ToListAsync();

        public async Task<bool> RollNumberExistsAsync(string rollNumber, int? excludeId = null) =>
            await _db.Students.AnyAsync(s => s.RollNumber == rollNumber && s.Id != excludeId);
    }
}

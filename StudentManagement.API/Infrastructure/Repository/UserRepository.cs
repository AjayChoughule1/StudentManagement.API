using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Domain.Entities;
using StudentManagement.API.Domain.Interfaces;
using StudentManagement.API.Infrastructure.Data;

namespace StudentManagement.API.Infrastructure.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext db) : base(db) { }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _db.Users.Include(u => u.Teacher)
                .FirstOrDefaultAsync(u => u.Email == email);

        public async Task<bool> EmailExistsAsync(string email) =>
            await _db.Users.AnyAsync(u => u.Email == email);
    }
}

using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Domain.Interfaces;
using StudentManagement.API.Infrastructure.Data;

namespace StudentManagement.API.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _db;
        protected readonly DbSet<T> _set;

        public Repository(AppDbContext db)
        {
            _db = db;
            _set = db.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id) => await _set.FindAsync(id);
        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _set.ToListAsync();
        public virtual async Task<T> AddAsync(T entity) { await _set.AddAsync(entity); return entity; }
        public virtual Task UpdateAsync(T entity) { _db.Entry(entity).State = EntityState.Modified; return Task.CompletedTask; }
        public virtual Task DeleteAsync(T entity) { _set.Remove(entity); return Task.CompletedTask; }
    }
}

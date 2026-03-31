using StudentManagement.API.Domain.Entities;

namespace StudentManagement.API.Domain.Interfaces
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        Task<Teacher?> GetWithSchedulesAsync(int teacherId);
        Task<IEnumerable<Teacher>> GetActiveTeachersAsync();
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    }
}

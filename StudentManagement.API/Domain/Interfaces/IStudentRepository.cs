using StudentManagement.API.Domain.Entities;

namespace StudentManagement.API.Domain.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<IEnumerable<Student>> GetByClassRoomAsync(int classRoomId);
        Task<Student?> GetByRollNumberAsync(string rollNumber);
        Task<IEnumerable<Student>> SearchAsync(string searchTerm);
        Task<bool> RollNumberExistsAsync(string rollNumber, int? excludeId = null);
    }
}

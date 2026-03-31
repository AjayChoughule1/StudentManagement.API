using StudentManagement.API.Domain.Entities;

namespace StudentManagement.API.Domain.Interfaces
{
    public interface IClassRoomRepository : IRepository<ClassRoom>
    {
        Task<ClassRoom?> GetWithStudentsAsync(int classRoomId);
        Task<ClassRoom?> GetWithSchedulesAsync(int classRoomId);
        Task<IEnumerable<ClassRoom>> GetActiveClassRoomsAsync();
    }
}

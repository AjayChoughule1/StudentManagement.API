using StudentManagement.API.Domain.Entities;

namespace StudentManagement.API.Domain.Interfaces
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<IEnumerable<Schedule>> GetByTeacherAsync(int teacherId);
        Task<IEnumerable<Schedule>> GetByClassRoomAsync(int classRoomId);
        Task<bool> HasConflictAsync(int teacherId, DayOfWeek day, TimeSpan start, TimeSpan end, int? excludeId = null);
    }
}

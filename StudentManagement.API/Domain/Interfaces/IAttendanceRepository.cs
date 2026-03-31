using StudentManagement.API.Domain.Entities;

namespace StudentManagement.API.Domain.Interfaces
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        Task<IEnumerable<Attendance>> GetByClassAndDateAsync(int classRoomId, DateTime date);
        Task<IEnumerable<Attendance>> GetByStudentAsync(int studentId, DateTime? from = null, DateTime? to = null);
        Task<IEnumerable<Attendance>> GetByClassAndMonthAsync(int classRoomId, int year, int month);
        Task<bool> AttendanceExistsAsync(int studentId, DateTime date);
        Task BulkInsertOrUpdateAsync(IEnumerable<Attendance> attendances);
    }
}

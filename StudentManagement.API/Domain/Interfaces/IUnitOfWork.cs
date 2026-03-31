namespace StudentManagement.API.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        ITeacherRepository Teachers { get; }
        IClassRoomRepository ClassRooms { get; }
        IScheduleRepository Schedules { get; }
        IAttendanceRepository Attendances { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}

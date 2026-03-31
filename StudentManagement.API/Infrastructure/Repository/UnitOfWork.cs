using StudentManagement.API.Domain.Interfaces;
using StudentManagement.API.Infrastructure.Data;

namespace StudentManagement.API.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public IStudentRepository Students { get; }
        public ITeacherRepository Teachers { get; }
        public IClassRoomRepository ClassRooms { get; }
        public IScheduleRepository Schedules { get; }
        public IAttendanceRepository Attendances { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Students = new StudentRepository(db);
            Teachers = new TeacherRepository(db);
            ClassRooms = new ClassRoomRepository(db);
            Schedules = new ScheduleRepository(db);
            Attendances = new AttendanceRepository(db);
            Users = new UserRepository(db);
        }

        public async Task<int> SaveChangesAsync() => await _db.SaveChangesAsync();

        public void Dispose() => _db.Dispose();
    }
}

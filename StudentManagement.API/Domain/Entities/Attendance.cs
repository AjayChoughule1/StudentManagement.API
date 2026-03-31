namespace StudentManagement.API.Domain.Entities
{
    public enum AttendanceStatus
    {
        Present = 1,
        Absent = 2,
        Leave = 3,
        HalfDay = 4
    }

    public class Attendance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public AttendanceStatus Status { get; set; }
        public string? Remarks { get; set; }
        public DateTime MarkedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int ClassRoomId { get; set; }
        public ClassRoom ClassRoom { get; set; } = null!;

        public int MarkedByTeacherId { get; set; }
        public Teacher MarkedByTeacher { get; set; } = null!;
    }
}

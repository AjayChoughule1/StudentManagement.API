namespace StudentManagement.API.Domain.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;

        public int ClassRoomId { get; set; }
        public ClassRoom ClassRoom { get; set; } = null!;
    }
}

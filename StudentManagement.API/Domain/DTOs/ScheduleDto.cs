namespace StudentManagement.API.Domain.DTOs
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string DayOfWeek { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public int ClassRoomId { get; set; }
        public string ClassRoomName { get; set; } = string.Empty;
    }

    public class CreateScheduleDto
    {
        public string Subject { get; set; } = string.Empty;
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int TeacherId { get; set; }
        public int ClassRoomId { get; set; }
    }
}

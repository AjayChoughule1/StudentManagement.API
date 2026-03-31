namespace StudentManagement.API.Domain.Entities
{
    public class ClassRoom
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;       // e.g. "Grade 10"
        public string Section { get; set; } = string.Empty;    // e.g. "A"
        public string? Description { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}

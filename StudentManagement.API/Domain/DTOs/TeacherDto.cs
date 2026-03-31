namespace StudentManagement.API.Domain.DTOs
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public bool IsActive { get; set; }
        public List<ScheduleDto> Schedules { get; set; } = new();
    }

    public class CreateTeacherDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        // Optional: create a user account for the teacher
        public string? Password { get; set; }
    }

    public class UpdateTeacherDto : CreateTeacherDto
    {
        public bool IsActive { get; set; }
    }
}

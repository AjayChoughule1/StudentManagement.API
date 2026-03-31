namespace StudentManagement.API.Domain.Entities
{
    public enum UserRole
    {
        Admin = 1,
        Teacher = 2
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        // Optional FK to Teacher (null if Admin)
        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
    }
}

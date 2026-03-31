using StudentManagement.API.Domain.Entities;

namespace StudentManagement.API.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            // Admin user
            if (!db.Users.Any(u => u.Role == UserRole.Admin))
            {
                db.Users.Add(new User
                {
                    Username = "admin",
                    Email = "admin@school.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = UserRole.Admin,
                    IsActive = true
                });
            }

            // Sample classes
            if (!db.ClassRooms.Any())
            {
                var classes = new[]
                {
                new ClassRoom { Name = "Grade 10", Section = "A", Capacity = 40 },
                new ClassRoom { Name = "Grade 10", Section = "B", Capacity = 40 },
                new ClassRoom { Name = "Grade 11", Section = "A", Capacity = 35 },
                new ClassRoom { Name = "Grade 11", Section = "B", Capacity = 35 },
            };
                db.ClassRooms.AddRange(classes);
            }

            // Sample teacher
            if (!db.Teachers.Any())
            {
                var teacher = new Teacher
                {
                    FirstName = "Rahul",
                    LastName = "Sharma",
                    Email = "rahul.sharma@school.com",
                    Phone = "9876543210",
                    Qualification = "M.Sc Mathematics"
                };
                db.Teachers.Add(teacher);
                await db.SaveChangesAsync();

                // Teacher login account
                db.Users.Add(new User
                {
                    Username = "rahul.sharma@school.com",
                    Email = "rahul.sharma@school.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"),
                    Role = UserRole.Teacher,
                    TeacherId = teacher.Id
                });
            }

            await db.SaveChangesAsync();
        }
    }
}

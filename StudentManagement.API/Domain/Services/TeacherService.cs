using StudentManagement.API.Domain.DTOs;
using StudentManagement.API.Domain.Entities;
using StudentManagement.API.Domain.Interfaces;

namespace StudentManagement.API.Domain.Services
{
    public class TeacherService
    {
        private readonly IUnitOfWork _uow;

        public TeacherService(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<TeacherDto>> GetAllAsync()
        {
            var teachers = await _uow.Teachers.GetActiveTeachersAsync();
            return teachers.Select(MapToDto);
        }

        public async Task<TeacherDto?> GetByIdAsync(int id)
        {
            var teacher = await _uow.Teachers.GetWithSchedulesAsync(id);
            return teacher is null ? null : MapToDto(teacher);
        }

        public async Task<TeacherDto> CreateAsync(CreateTeacherDto dto)
        {
            if (await _uow.Teachers.EmailExistsAsync(dto.Email))
                throw new InvalidOperationException($"Email '{dto.Email}' is already registered.");

            var teacher = new Teacher
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Qualification = dto.Qualification,
                PhotoUrl = dto.PhotoUrl
            };

            await _uow.Teachers.AddAsync(teacher);
            await _uow.SaveChangesAsync();

            // Optionally create a login account for the teacher
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var user = new User
                {
                    Username = dto.Email,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Role = UserRole.Teacher,
                    TeacherId = teacher.Id
                };
                await _uow.Users.AddAsync(user);
                await _uow.SaveChangesAsync();
            }

            return MapToDto(teacher);
        }

        public async Task<TeacherDto> UpdateAsync(int id, UpdateTeacherDto dto)
        {
            var teacher = await _uow.Teachers.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Teacher {id} not found.");

            if (await _uow.Teachers.EmailExistsAsync(dto.Email, id))
                throw new InvalidOperationException($"Email '{dto.Email}' already in use.");

            teacher.FirstName = dto.FirstName;
            teacher.LastName = dto.LastName;
            teacher.Email = dto.Email;
            teacher.Phone = dto.Phone;
            teacher.Qualification = dto.Qualification;
            teacher.PhotoUrl = dto.PhotoUrl;
            teacher.IsActive = dto.IsActive;

            await _uow.Teachers.UpdateAsync(teacher);
            await _uow.SaveChangesAsync();
            return MapToDto(teacher);
        }

        public async Task DeleteAsync(int id)
        {
            var teacher = await _uow.Teachers.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Teacher {id} not found.");
            await _uow.Teachers.DeleteAsync(teacher);
            await _uow.SaveChangesAsync();
        }

        private static TeacherDto MapToDto(Teacher t) => new()
        {
            Id = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Email = t.Email,
            Phone = t.Phone,
            Qualification = t.Qualification,
            PhotoUrl = t.PhotoUrl,
            IsActive = t.IsActive,
            Schedules = t.Schedules.Select(s => new ScheduleDto
            {
                Id = s.Id,
                Subject = s.Subject,
                DayOfWeek = s.DayOfWeek.ToString(),
                StartTime = s.StartTime.ToString(@"hh\:mm"),
                EndTime = s.EndTime.ToString(@"hh\:mm"),
                TeacherId = s.TeacherId,
                ClassRoomId = s.ClassRoomId,
                ClassRoomName = s.ClassRoom?.Name + " " + s.ClassRoom?.Section ?? string.Empty
            }).ToList()
        };
    }
}

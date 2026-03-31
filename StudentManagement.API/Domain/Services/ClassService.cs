using StudentManagement.API.Domain.DTOs;
using StudentManagement.API.Domain.Entities;
using StudentManagement.API.Domain.Interfaces;

namespace StudentManagement.API.Domain.Services
{
    public class ClassService
    {
        private readonly IUnitOfWork _uow;

        public ClassService(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<ClassRoomDto>> GetAllAsync()
        {
            var classes = await _uow.ClassRooms.GetActiveClassRoomsAsync();
            return classes.Select(MapToDto);
        }

        public async Task<ClassRoomDto?> GetByIdAsync(int id)
        {
            var classRoom = await _uow.ClassRooms.GetWithSchedulesAsync(id);
            if (classRoom is null) return null;
            var dto = MapToDto(classRoom);
            dto.StudentCount = (await _uow.Students.GetByClassRoomAsync(id)).Count();
            return dto;
        }

        public async Task<ClassRoomDto> CreateAsync(CreateClassRoomDto dto)
        {
            var classRoom = new ClassRoom
            {
                Name = dto.Name,
                Section = dto.Section,
                Description = dto.Description,
                Capacity = dto.Capacity
            };
            await _uow.ClassRooms.AddAsync(classRoom);
            await _uow.SaveChangesAsync();
            return MapToDto(classRoom);
        }

        public async Task<ClassRoomDto> UpdateAsync(int id, UpdateClassRoomDto dto)
        {
            var classRoom = await _uow.ClassRooms.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"ClassRoom {id} not found.");

            classRoom.Name = dto.Name;
            classRoom.Section = dto.Section;
            classRoom.Description = dto.Description;
            classRoom.Capacity = dto.Capacity;
            classRoom.IsActive = dto.IsActive;

            await _uow.ClassRooms.UpdateAsync(classRoom);
            await _uow.SaveChangesAsync();
            return MapToDto(classRoom);
        }

        public async Task DeleteAsync(int id)
        {
            var classRoom = await _uow.ClassRooms.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"ClassRoom {id} not found.");
            await _uow.ClassRooms.DeleteAsync(classRoom);
            await _uow.SaveChangesAsync();
        }

        private static ClassRoomDto MapToDto(ClassRoom c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            Section = c.Section,
            Description = c.Description,
            Capacity = c.Capacity,
            IsActive = c.IsActive,
            Schedules = c.Schedules.Select(s => new ScheduleDto
            {
                Id = s.Id,
                Subject = s.Subject,
                DayOfWeek = s.DayOfWeek.ToString(),
                StartTime = s.StartTime.ToString(@"hh\:mm"),
                EndTime = s.EndTime.ToString(@"hh\:mm"),
                TeacherId = s.TeacherId,
                TeacherName = s.Teacher?.FirstName + " " + s.Teacher?.LastName ?? string.Empty,
                ClassRoomId = s.ClassRoomId
            }).ToList()
        };
    }

    // ─────────────────────── Schedule Service ───────────────────────
    public class ScheduleService
    {
        private readonly IUnitOfWork _uow;

        public ScheduleService(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<ScheduleDto>> GetByTeacherAsync(int teacherId)
        {
            var schedules = await _uow.Schedules.GetByTeacherAsync(teacherId);
            return schedules.Select(MapToDto);
        }

        public async Task<IEnumerable<ScheduleDto>> GetByClassAsync(int classRoomId)
        {
            var schedules = await _uow.Schedules.GetByClassRoomAsync(classRoomId);
            return schedules.Select(MapToDto);
        }

        public async Task<ScheduleDto> CreateAsync(CreateScheduleDto dto)
        {
            _ = await _uow.Teachers.GetByIdAsync(dto.TeacherId)
                ?? throw new KeyNotFoundException($"Teacher {dto.TeacherId} not found.");
            _ = await _uow.ClassRooms.GetByIdAsync(dto.ClassRoomId)
                ?? throw new KeyNotFoundException($"ClassRoom {dto.ClassRoomId} not found.");

            if (await _uow.Schedules.HasConflictAsync(dto.TeacherId, dto.DayOfWeek, dto.StartTime, dto.EndTime))
                throw new InvalidOperationException("Teacher has a conflicting schedule for this time slot.");

            var schedule = new Schedule
            {
                Subject = dto.Subject,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                TeacherId = dto.TeacherId,
                ClassRoomId = dto.ClassRoomId
            };

            await _uow.Schedules.AddAsync(schedule);
            await _uow.SaveChangesAsync();
            return MapToDto(schedule);
        }

        public async Task DeleteAsync(int id)
        {
            var schedule = await _uow.Schedules.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Schedule {id} not found.");
            await _uow.Schedules.DeleteAsync(schedule);
            await _uow.SaveChangesAsync();
        }

        private static ScheduleDto MapToDto(Schedule s) => new()
        {
            Id = s.Id,
            Subject = s.Subject,
            DayOfWeek = s.DayOfWeek.ToString(),
            StartTime = s.StartTime.ToString(@"hh\:mm"),
            EndTime = s.EndTime.ToString(@"hh\:mm"),
            TeacherId = s.TeacherId,
            TeacherName = s.Teacher is null ? string.Empty : $"{s.Teacher.FirstName} {s.Teacher.LastName}",
            ClassRoomId = s.ClassRoomId,
            ClassRoomName = s.ClassRoom is null ? string.Empty : $"{s.ClassRoom.Name} {s.ClassRoom.Section}"
        };
    }
}

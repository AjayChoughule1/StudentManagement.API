using StudentManagement.API.Domain.DTOs;
using StudentManagement.API.Domain.Entities;
using StudentManagement.API.Domain.Interfaces;

namespace StudentManagement.API.Domain.Services
{
    public class StudentService
    {
        private readonly IUnitOfWork _uow;

        public StudentService(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<StudentDto>> GetAllAsync()
        {
            var students = await _uow.Students.GetAllAsync();
            return students.Select(MapToDto);
        }

        public async Task<StudentDto?> GetByIdAsync(int id)
        {
            var student = await _uow.Students.GetByIdAsync(id);
            return student is null ? null : MapToDto(student);
        }

        public async Task<IEnumerable<StudentDto>> GetByClassAsync(int classRoomId)
        {
            var students = await _uow.Students.GetByClassRoomAsync(classRoomId);
            return students.Select(MapToDto);
        }

        public async Task<IEnumerable<StudentDto>> SearchAsync(string term)
        {
            var students = await _uow.Students.SearchAsync(term);
            return students.Select(MapToDto);
        }

        public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
        {
            if (await _uow.Students.RollNumberExistsAsync(dto.RollNumber))
                throw new InvalidOperationException($"Roll number '{dto.RollNumber}' already exists.");

            var classRoom = await _uow.ClassRooms.GetByIdAsync(dto.ClassRoomId)
                ?? throw new KeyNotFoundException($"ClassRoom {dto.ClassRoomId} not found.");

            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                RollNumber = dto.RollNumber,
                Email = dto.Email,
                Phone = dto.Phone,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Address = dto.Address,
                PhotoUrl = dto.PhotoUrl,
                ClassRoomId = dto.ClassRoomId
            };

            await _uow.Students.AddAsync(student);
            await _uow.SaveChangesAsync();
            return MapToDto(student);
        }

        public async Task<StudentDto> UpdateAsync(int id, UpdateStudentDto dto)
        {
            var student = await _uow.Students.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Student {id} not found.");

            if (await _uow.Students.RollNumberExistsAsync(dto.RollNumber, id))
                throw new InvalidOperationException($"Roll number '{dto.RollNumber}' already in use.");

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.RollNumber = dto.RollNumber;
            student.Email = dto.Email;
            student.Phone = dto.Phone;
            student.DateOfBirth = dto.DateOfBirth;
            student.Gender = dto.Gender;
            student.Address = dto.Address;
            student.PhotoUrl = dto.PhotoUrl;
            student.ClassRoomId = dto.ClassRoomId;
            student.IsActive = dto.IsActive;

            await _uow.Students.UpdateAsync(student);
            await _uow.SaveChangesAsync();
            return MapToDto(student);
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _uow.Students.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Student {id} not found.");
            await _uow.Students.DeleteAsync(student);
            await _uow.SaveChangesAsync();
        }

        private static StudentDto MapToDto(Student s) => new()
        {
            Id = s.Id,
            FirstName = s.FirstName,
            LastName = s.LastName,
            RollNumber = s.RollNumber,
            Email = s.Email,
            Phone = s.Phone,
            DateOfBirth = s.DateOfBirth,
            Gender = s.Gender,
            Address = s.Address,
            PhotoUrl = s.PhotoUrl,
            IsActive = s.IsActive,
            ClassRoomId = s.ClassRoomId,
            ClassRoomName = s.ClassRoom?.Name + " " + s.ClassRoom?.Section ?? string.Empty
        };
    }
}

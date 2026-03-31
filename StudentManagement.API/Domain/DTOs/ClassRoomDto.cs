namespace StudentManagement.API.Domain.DTOs
{
    public class ClassRoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public int StudentCount { get; set; }
        public List<ScheduleDto> Schedules { get; set; } = new();
    }

    public class CreateClassRoomDto
    {
        public string Name { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Capacity { get; set; }
    }

    public class UpdateClassRoomDto : CreateClassRoomDto
    {
        public bool IsActive { get; set; }
    }
}

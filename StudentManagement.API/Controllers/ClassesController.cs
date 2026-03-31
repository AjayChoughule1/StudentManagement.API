using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Domain.DTOs;
using StudentManagement.API.Domain.Services;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ClassesController : ControllerBase
    {
        private readonly ClassService _classSvc;
        private readonly ScheduleService _schedSvc;

        public ClassesController(ClassService classSvc, ScheduleService schedSvc)
        {
            _classSvc = classSvc;
            _schedSvc = schedSvc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _classSvc.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var c = await _classSvc.GetByIdAsync(id);
            return c is null ? NotFound() : Ok(c);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateClassRoomDto dto)
        {
            var created = await _classSvc.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClassRoomDto dto) =>
            Ok(await _classSvc.UpdateAsync(id, dto));

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _classSvc.DeleteAsync(id);
            return NoContent();
        }
        // ── Schedules (nested under classes) ──────────────────────
        [HttpGet("{classId}/schedules")]
        public async Task<IActionResult> GetSchedules(int classId) =>
            Ok(await _schedSvc.GetByClassAsync(classId));

        [HttpPost("{classId}/schedules")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSchedule(int classId, [FromBody] CreateScheduleDto dto)
        {
            dto.ClassRoomId = classId;
            var created = await _schedSvc.CreateAsync(dto);
            return Ok(created);
        }

        [HttpDelete("schedules/{scheduleId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSchedule(int scheduleId)
        {
            await _schedSvc.DeleteAsync(scheduleId);
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Domain.DTOs;
using StudentManagement.API.Domain.Services;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _svc;
        public StudentsController(StudentService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var s = await _svc.GetByIdAsync(id);
            return s is null ? NotFound() : Ok(s);
        }

        [HttpGet("class/{classRoomId}")]
        public async Task<IActionResult> GetByClass(int classRoomId) =>
            Ok(await _svc.GetByClassAsync(classRoomId));

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term) =>
            Ok(await _svc.SearchAsync(term));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentDto dto) =>
            Ok(await _svc.UpdateAsync(id, dto));

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteAsync(id);
            return NoContent();
        }
    }
}

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
    public class AttendanceController : ControllerBase
    {
        private readonly AttendanceService _svc;
        public AttendanceController(AttendanceService svc) => _svc = svc;

        /// GET /api/attendance/class/{classRoomId}?date=2025-03-01
        [HttpGet("class/{classRoomId}")]
        public async Task<IActionResult> GetByClassAndDate(int classRoomId, [FromQuery] DateTime date) =>
            Ok(await _svc.GetByClassAndDateAsync(classRoomId, date));

        /// GET /api/attendance/student/{studentId}?from=2025-01-01&to=2025-01-31
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudent(
            int studentId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to) =>
            Ok(await _svc.GetByStudentAsync(studentId, from, to));

        /// POST /api/attendance/bulk  — teacher marks full class attendance
        [HttpPost("bulk")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> BulkMark([FromBody] BulkMarkAttendanceDto dto)
        {
            await _svc.BulkMarkAsync(dto);
            return Ok(new { message = "Attendance marked successfully." });
        }

        /// GET /api/attendance/report/{classRoomId}?year=2025&month=3
        [HttpGet("report/{classRoomId}")]
        public async Task<IActionResult> GetMonthlyReport(
            int classRoomId,
            [FromQuery] int year,
            [FromQuery] int month) =>
            Ok(await _svc.GetMonthlyReportAsync(classRoomId, year, month));
    }
}

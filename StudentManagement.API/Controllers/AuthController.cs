using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Domain.DTOs;
using StudentManagement.API.Domain.Services;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login with email + password — returns a JWT token
        /// POST /api/auth/login
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Get currently logged-in user info from the JWT token
        /// GET /api/auth/me
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var teacherId = User.FindFirst("TeacherId")?.Value;

            return Ok(new
            {
                UserId = userId,
                Email = email,
                Username = username,
                Role = role,
                TeacherId = string.IsNullOrEmpty(teacherId) ? (int?)null : int.Parse(teacherId)
            });
        }
    }
}

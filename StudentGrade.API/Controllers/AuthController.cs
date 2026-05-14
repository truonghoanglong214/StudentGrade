using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentGrade.Application.DTOs.AuthDtos;
using StudentGrade.Application.Interfaces.IServices;
using StudentGrade.API.Helpers;
using System.Threading.Tasks;

namespace StudentGrade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICurrentUserHelper _currentUserHelper;

        public AuthController(IAuthService authService, ICurrentUserHelper currentUserHelper)
        {
            _authService = authService;
            _currentUserHelper = currentUserHelper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = _currentUserHelper.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _authService.GetCurrentUserAsync(userId.Value);
            return Ok(user);
        }
    }
}

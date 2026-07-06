using Microsoft.AspNetCore.Mvc;
using LibraryExercise.Api.Services;
using LibraryExercise.Api.Models;
using LibraryExercise.Api.DTOs;

namespace LibraryExercise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                await _authService.Register(request);
                return Ok(new { message = "Register successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.Login(request);

            if (!result.Success)
                return Unauthorized(new { message = result.Message });

            return Ok(new
            {
                message = result.Message,
                token = result.Token,
                userId = result.UserId,
                name = result.UserName
            });
        }
    }
}
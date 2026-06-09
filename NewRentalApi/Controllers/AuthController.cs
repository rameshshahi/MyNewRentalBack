using Microsoft.AspNetCore.Mvc;
using NewRentalApi.DTOs;
using NewRentalApi.Services;

namespace NewRentalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(
            RegisterDto dto)
        {
            var result =
                await _authService.RegisterAsync(dto);

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(
            LoginDto dto)
        {
            var result =
                await _authService.LoginAsync(dto);

            return Ok(result);
        }
    }
}

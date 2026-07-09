using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Application.DTOs.Auth;
using TeamTaskManager.Application.Interfaces.Services;

namespace TeamTaskManager.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto Rdto)
        {
            var res = await _authService.RegisterAsync(Rdto);
            if (!res.IsSuccess)
                return BadRequest(res.ErrorMessage);
            return Ok(res.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto Ldto)
        {
            var res = await _authService.LoginAsync(Ldto);
            if (!res.IsSuccess)
                return Unauthorized(res.ErrorMessage);
            return Ok(res.Data);
        }
    }
}

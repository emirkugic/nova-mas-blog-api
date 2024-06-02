using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using nova_mas_blog_api.DTOs.AuthDTO;
using nova_mas_blog_api.Services;
using System.Threading.Tasks;

namespace nova_mas_blog_api.Controllers
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var result = await _authService.Register(registerDto);
            if (!result)
            {
                return BadRequest("User already exists.");
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var token = await _authService.Login(loginDto);
            if (token == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(new { Token = token });
        }
    }
}

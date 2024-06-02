using Microsoft.AspNetCore.Mvc;
using nova_mas_blog_api.DTOs.AuthDTO;
using nova_mas_blog_api.Services;

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
            try
            {
                var result = await _authService.Register(registerDto);
                if (!result)
                {
                    return BadRequest("User already exists.");
                }

                return StatusCode(201);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong. Please try again later.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var token = await _authService.Login(loginDto);
                if (token == null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                return Ok(new { Token = token });
            }
            catch (System.Exception)
            {
                return BadRequest("Something went wrong. Please try again later.");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using nova_mas_blog_api.DTOs.UserDTOs;
using nova_mas_blog_api.Services;

namespace nova_mas_blog_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }


        [HttpGet("all")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAll(1, int.MaxValue);
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the users. Please try again later.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int page = 1, int pageSize = 10)
        {
            try
            {
                var users = await _userService.GetAllUsers(page, pageSize);
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the users. Please try again later.");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the user. Please try again later.");
            }
        }

        [HttpPost]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO dto)
        {
            try
            {
                var user = await _userService.CreateUser(dto);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the user. Please try again later.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDTO dto)
        {
            try
            {
                var user = await _userService.UpdateUser(id, dto);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the user. Please try again later.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var success = await _userService.Delete(id);
                if (!success)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the user. Please try again later.");
            }
        }
    }
}

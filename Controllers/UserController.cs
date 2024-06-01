#pragma warning disable CS0168 // To suppress the warning about the unused variable e in the catch block

using Microsoft.AspNetCore.Mvc;
using nova_mas_blog_api.Data;
using nova_mas_blog_api.Models;
using MongoDB.Driver;
using nova_mas_blog_api.DTOs.UserDTOs;
using AutoMapper;

namespace nova_mas_blog_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly MongoDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(MongoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var usersQuery = _context.Users.Find(_ => true);
            var totalItems = await usersQuery.CountDocumentsAsync();
            var users = await usersQuery.Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

            var response = new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize),
                Items = users
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _context.Users.Find<User>(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO dto)
        {
            try
            {
                var existingUser = await _context.Users.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();
                if (existingUser != null)
                {
                    return Conflict("A user with this email already exists.");
                }

                var user = _mapper.Map<User>(dto);
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                user.CreatedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.Users.InsertOneAsync(user);
                return StatusCode(201, "User created successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An error occurred while creating the user. Please try again later.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDTO dto)
        {
            try
            {
                var userToUpdate = await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
                if (userToUpdate == null)
                {
                    return NotFound("User not found.");
                }

                if (dto.Email != null && dto.Email != userToUpdate.Email)
                {
                    var existingUserWithEmail = await _context.Users.Find(u => u.Email == dto.Email && u.Id != id).FirstOrDefaultAsync();
                    if (existingUserWithEmail != null)
                    {
                        return Conflict("Email is already in use by another account.");
                    }
                    userToUpdate.Email = dto.Email;
                }

                // Update fields only if they are not null
                _mapper.Map(dto, userToUpdate);

                if (dto.Password != null)
                {
                    userToUpdate.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                }

                userToUpdate.UpdatedAt = DateTime.UtcNow;

                var result = await _context.Users.ReplaceOneAsync(u => u.Id == id, userToUpdate);
                if (result.ModifiedCount == 0)
                {
                    return NotFound();
                }

                return StatusCode(201, "User updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the user. Please try again later.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _context.Users.DeleteOneAsync(u => u.Id == id);
            if (result.DeletedCount == 0)
            {
                return NotFound();
            }
            return StatusCode(204);
        }
    }
}

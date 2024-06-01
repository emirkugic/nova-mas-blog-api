using AutoMapper;
using MongoDB.Driver;
using nova_mas_blog_api.Data;
using nova_mas_blog_api.DTOs.UserDTOs;
using nova_mas_blog_api.Models;

namespace nova_mas_blog_api.Services
{
    public class UserService : IUserService
    {
        private readonly MongoDbContext _context;
        private readonly IMapper _mapper;

        public UserService(MongoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetUsers(int page, int pageSize)
        {
            var usersQuery = _context.Users.Find(_ => true);
            return await usersQuery.Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();
        }

        public async Task<User> GetUserById(string id)
        {
            return await _context.Users.Find<User>(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> CreateUser(UserCreateDTO dto)
        {
            var existingUser = await _context.Users.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.Users.InsertOneAsync(user);
            return user;
        }

        public async Task<User> UpdateUser(string id, UserUpdateDTO dto)
        {
            var userToUpdate = await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (userToUpdate == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (dto.Email != null && dto.Email != userToUpdate.Email)
            {
                var existingUserWithEmail = await _context.Users.Find(u => u.Email == dto.Email && u.Id != id).FirstOrDefaultAsync();
                if (existingUserWithEmail != null)
                {
                    throw new InvalidOperationException("Email is already in use by another account.");
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
                throw new InvalidOperationException("Failed to update the user.");
            }

            return userToUpdate;
        }

        public async Task<bool> DeleteUser(string id)
        {
            var result = await _context.Users.DeleteOneAsync(u => u.Id == id);
            return result.DeletedCount > 0;
        }
    }
}

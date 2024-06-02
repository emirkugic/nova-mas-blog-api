using AutoMapper;
using MongoDB.Driver;
using nova_mas_blog_api.Data;
using nova_mas_blog_api.DTOs.UserDTOs;
using nova_mas_blog_api.Models;

namespace nova_mas_blog_api.Services
{
    public class UserService : BaseService<User>
    {
        private readonly IMapper _mapper;

        public UserService(MongoDbContext context, IMapper mapper) : base(context.Users)
        {
            _mapper = mapper;
        }

        public async Task<User> CreateUser(UserCreateDTO dto)
        {

            // Sanitize input data
            dto.FirstName = SanitizationHelper.SanitizeInput(dto.FirstName);
            dto.LastName = SanitizationHelper.SanitizeInput(dto.LastName);
            dto.Username = SanitizationHelper.SanitizeInput(dto.Username);
            dto.Email = SanitizationHelper.SanitizeInput(dto.Email);


            var existingUser = await _collection.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            return await Create(user);
        }

        public async Task<User> UpdateUser(string id, UserUpdateDTO dto)
        {
            var userToUpdate = await GetById(id);

            // Sanitize input data
            if (dto.FirstName != null)
            {
                dto.FirstName = SanitizationHelper.SanitizeInput(dto.FirstName);
            }
            if (dto.LastName != null)
            {
                dto.LastName = SanitizationHelper.SanitizeInput(dto.LastName);
            }
            if (dto.Username != null)
            {
                dto.Username = SanitizationHelper.SanitizeInput(dto.Username);
            }
            if (dto.Email != null)
            {
                dto.Email = SanitizationHelper.SanitizeInput(dto.Email);
            }

            _mapper.Map(dto, userToUpdate);

            if (dto.Password != null)
            {
                userToUpdate.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            userToUpdate.UpdatedAt = DateTime.UtcNow;

            return await Update(id, userToUpdate);
        }
    }
}

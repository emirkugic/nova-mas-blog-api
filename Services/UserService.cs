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


        public async Task<List<UserReadDTO>> GetAllUsers(int page, int pageSize)
        {
            var users = await _collection.Find(_ => true)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
            return _mapper.Map<List<UserReadDTO>>(users);
        }

        public async Task<UserReadDTO> GetUserById(string id)
        {
            var user = await _collection.Find(Builders<User>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
            return _mapper.Map<UserReadDTO>(user);
        }

        public async Task<User> CreateUser(UserCreateDTO dto)
        {
            SanitizeUserDto(dto);

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
            SanitizeUserDto(dto);

            _mapper.Map(dto, userToUpdate);

            if (dto.Password != null)
            {
                userToUpdate.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            userToUpdate.UpdatedAt = DateTime.UtcNow;

            return await Update(id, userToUpdate);
        }


        // * ||--------------------------------------------------------------------------------||
        // * ||                                 Helper method                                  ||
        // * ||--------------------------------------------------------------------------------||

        private void SanitizeUserDto(dynamic dto)
        {
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
        }
    }
}

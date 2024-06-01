using nova_mas_blog_api.DTOs.UserDTOs;
using nova_mas_blog_api.Models;

namespace nova_mas_blog_api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers(int page, int pageSize);
        Task<User> GetUserById(string id);
        Task<User> CreateUser(UserCreateDTO dto);
        Task<User> UpdateUser(string id, UserUpdateDTO dto);
        Task<bool> DeleteUser(string id);
    }
}

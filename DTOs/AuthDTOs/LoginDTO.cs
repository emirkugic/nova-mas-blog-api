namespace nova_mas_blog_api.DTOs.AuthDTO
{
    public class LoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}

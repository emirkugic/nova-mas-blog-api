namespace nova_mas_blog_api.DTOs.AuthDTO
{
    public class RegisterDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}

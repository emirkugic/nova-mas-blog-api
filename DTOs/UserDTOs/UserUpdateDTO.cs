namespace nova_mas_blog_api.DTOs.UserDTOs
{
    public class UserUpdateDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }

}

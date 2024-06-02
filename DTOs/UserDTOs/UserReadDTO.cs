namespace nova_mas_blog_api.DTOs.UserDTOs
{
    public class UserReadDTO
    {
        public required string Id { get; set; }
        public required string FullName { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string ProfilePictureUrl { get; set; }
    }
}

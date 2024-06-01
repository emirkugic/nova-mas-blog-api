using nova_mas_blog_api.Enums;
using System.ComponentModel.DataAnnotations;


namespace nova_mas_blog_api.DTOs.UserDTOs
{

    public class UserCreateDTO
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required, StringLength(100, MinimumLength = 2)]
        public required string LastName { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        public required string Username { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public bool IsEmailConfirmed { get; set; } = false;
        public bool TwoFactorEnabled { get; set; } = false;
        public string TwoFactorSecret { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/Default_pfp.svg/2048px-Default_pfp.svg.png";
    }

}
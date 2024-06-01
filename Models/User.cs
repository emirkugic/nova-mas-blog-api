using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using nova_mas_blog_api.Enums;
using System.ComponentModel.DataAnnotations;

namespace nova_mas_blog_api.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("firstName"), Required, StringLength(100, MinimumLength = 1)]
        public required string FirstName { get; set; }

        [BsonElement("lastName"), Required, StringLength(100, MinimumLength = 1)]
        public required string LastName { get; set; }

        [BsonElement("username"), Required, StringLength(100, MinimumLength = 3)]
        public required string Username { get; set; }

        [BsonElement("email"), Required, EmailAddress]
        public required string Email { get; set; }

        [BsonElement("passwordHash"), Required]
        public required string PasswordHash { get; set; }

        [BsonElement("role"), Required]
        public required UserRole Role { get; set; }

        [BsonElement("isEmailConfirmed")]
        public required bool IsEmailConfirmed { get; set; }

        [BsonElement("twoFactorEnabled")]
        public required bool TwoFactorEnabled { get; set; }

        [BsonElement("twoFactorSecret")]
        public required string TwoFactorSecret { get; set; }

        [BsonElement("securityQuestions")]
        public required List<string> SecurityQuestions { get; set; }

        [BsonElement("profilePictureUrl")]
        public required string ProfilePictureUrl { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; private set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

}


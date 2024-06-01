using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using nova_mas_blog_api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace nova_mas_blog_api.Models
{

    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required, StringLength(100, MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required, StringLength(100, MinimumLength = 2)]
        public required string LastName { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        public required string Username { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public required string TwoFactorSecret { get; set; }

        public required string ProfilePictureUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using nova_mas_blog_api.Enums;

namespace nova_mas_blog_api.Models
{
    public class Blog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required List<string> ImageUrls { get; set; }
        public required List<string> VideoUrls { get; set; }
        public BlogCategory Category { get; set; }
        public DateTime DateCreated { get; set; }
        public int ViewCount { get; set; }
        public bool IsFeatured { get; set; }
        public required string user_id { get; set; }

    }
}

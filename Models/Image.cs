using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace nova_mas_blog_api.Models
{
    public class Image
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string Url { get; set; }
        public required string DeleteHash { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UploadDate { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public required string BlogId { get; set; }
    }
}

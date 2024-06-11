namespace nova_mas_blog_api.DTOs
{

    public class ImageResponseDTO
    {
        public required string Id { get; set; }
        public required string Url { get; set; }
        public required string DeleteHash { get; set; }
        public DateTime UploadDate { get; set; }
        public required string BlogId { get; set; }  // The ID of the blog associated with this image
    }
}

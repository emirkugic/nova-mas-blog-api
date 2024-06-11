namespace nova_mas_blog_api.DTOs
{
    public class ImageUploadDTO
    {
        public required List<IFormFile> ImageFiles { get; set; }
        public required string BlogId { get; set; }
    }
}

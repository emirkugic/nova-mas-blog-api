using nova_mas_blog_api.Enums;


namespace nova_mas_blog_api.DTOs.BlogDTOs
{
    public class BlogCreateDTO
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public BlogCategory Category { get; set; }
        public required string UserId { get; set; }
    }
}

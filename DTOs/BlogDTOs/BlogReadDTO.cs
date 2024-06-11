using nova_mas_blog_api.Enums;


namespace nova_mas_blog_api.DTOs.BlogDTOs
{
    public class BlogReadDTO
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public BlogCategory Category { get; set; }
        public DateTime DateCreated { get; set; }
        public int ViewCount { get; set; }
        public bool IsFeatured { get; set; }
        public required string FullName { get; set; }
    }
}
using nova_mas_blog_api.Models;
using MongoDB.Driver;
using nova_mas_blog_api.Enums;
using nova_mas_blog_api.Data;

namespace nova_mas_blog_api.Services
{
    public class BlogService : BaseService<Blog>
    {
        public BlogService(MongoDbContext context) : base(context.Blogs) { }

        public async Task<IEnumerable<Blog>> GetByUserId(string userId, int page, int pageSize)
        {
            return await _collection.Find(blog => blog.user_id == userId)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Blog>> GetFeaturedBlogs(int page, int pageSize)
        {
            return await _collection.Find(blog => blog.IsFeatured)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Blog>> GetBlogsByCategory(BlogCategory category, int page, int pageSize)
        {
            return await _collection.Find(blog => blog.Category == category)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        // Method to sort blogs, change `sortBy` to match your sorting requirements
        public async Task<IEnumerable<Blog>> GetSortedBlogs(string sortBy, int page, int pageSize)
        {
            var sortDefinition = sortBy switch
            {
                "date" => Builders<Blog>.Sort.Descending(blog => blog.DateCreated),
                "views" => Builders<Blog>.Sort.Descending(blog => blog.ViewCount),
                _ => Builders<Blog>.Sort.Ascending(blog => blog.Title)
            };

            return await _collection.Find(_ => true)
                                    .Sort(sortDefinition)
                                    .Skip((page - 1) * pageSize)
                                    .Limit(pageSize)
                                    .ToListAsync();
        }

        public async Task<IEnumerable<Blog>> GetBlogsByUserId(string userId, int page, int pageSize)
        {
            var filter = Builders<Blog>.Filter.Eq(blog => blog.user_id, userId);
            return await _collection.Find(filter)
                                    .Skip((page - 1) * pageSize)
                                    .Limit(pageSize)
                                    .ToListAsync();
        }

    }
}
using nova_mas_blog_api.Models;
using MongoDB.Driver;
using nova_mas_blog_api.Enums;
using nova_mas_blog_api.Data;
using MongoDB.Bson;

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


        // ! ||--------------------------------------------------------------------------------||
        // ! ||                                  Fancy Search                                  ||
        // ! ||--------------------------------------------------------------------------------||

        public async Task<IEnumerable<Blog>> SearchBlogs(
         string searchText,
         BlogCategory? category,
         bool? isFeatured,
         string sortBy,
         bool isAscending,
         string userId,
         string nameSearch,
         int page,
         int pageSize)
        {
            var filterBuilder = Builders<Blog>.Filter;
            var filter = filterBuilder.Empty;

            // Text search on title or content
            if (!string.IsNullOrEmpty(searchText))
            {
                var textFilter = filterBuilder.Regex("Title", new BsonRegularExpression(searchText, "i")) |
                                 filterBuilder.Regex("Content", new BsonRegularExpression(searchText, "i"));
                filter &= textFilter;
            }

            // Filter by category
            if (category.HasValue)
            {
                filter &= filterBuilder.Eq("Category", category.Value);
            }

            // Filter by featured status
            if (isFeatured.HasValue)
            {
                filter &= filterBuilder.Eq("IsFeatured", isFeatured.Value);
            }

            // Filter by user ID
            if (!string.IsNullOrEmpty(userId))
            {
                filter &= filterBuilder.Eq("user_id", userId);
            }

            // Search by user's name 
            if (!string.IsNullOrEmpty(nameSearch))
            {
                // TODO: Implement search by user's name
            }

            // Apply sorting based on the sortBy parameter and direction
            var sortDefinition = GetSortDefinition(sortBy, isAscending);

            return await _collection.Find(filter)
                .Sort(sortDefinition)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        private SortDefinition<Blog> GetSortDefinition(string sortBy, bool isAscending)
        {
            var sortBuilder = Builders<Blog>.Sort;
            return sortBy switch
            {
                "date" => isAscending ? sortBuilder.Ascending("DateCreated") : sortBuilder.Descending("DateCreated"),
                "viewCount" => isAscending ? sortBuilder.Ascending("ViewCount") : sortBuilder.Descending("ViewCount"),
                "userId" => isAscending ? sortBuilder.Ascending("user_id") : sortBuilder.Descending("user_id"),
                _ => sortBuilder.Ascending("Title")
            };
        }

    }
}
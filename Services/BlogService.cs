using nova_mas_blog_api.Models;
using MongoDB.Driver;
using nova_mas_blog_api.Enums;
using nova_mas_blog_api.Data;
using MongoDB.Bson;
using nova_mas_blog_api.DTOs.BlogDTOs;

namespace nova_mas_blog_api.Services
{
    public class BlogService : BaseService<Blog>
    {
        private readonly UserService _userService;
        public BlogService(MongoDbContext context, UserService userService) : base(context.Blogs)
        {
            _userService = userService;
        }


        public async Task<IEnumerable<Blog>> GetByUserId(string userId, int page, int pageSize)
        {
            return await _collection.Find(blog => blog.UserId == userId)
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


        // ! ||--------------------------------------------------------------------------------||
        // ! ||                                  Fancy Search                                  ||
        // ! ||--------------------------------------------------------------------------------||

        public async Task<long> GetBlogsCount(FilterDefinition<Blog> filter)
        {
            return await _collection.CountDocumentsAsync(filter);
        }

        public async Task<(IEnumerable<BlogReadDTO>, long)> SearchBlogs(
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

            if (!string.IsNullOrEmpty(searchText))
            {
                var textFilter = filterBuilder.Regex("Title", new BsonRegularExpression(searchText, "i")) |
                                 filterBuilder.Regex("Content", new BsonRegularExpression(searchText, "i"));
                filter &= textFilter;
            }

            if (category.HasValue)
            {
                filter &= filterBuilder.Eq("Category", category.Value);
            }

            if (isFeatured.HasValue)
            {
                filter &= filterBuilder.Eq("IsFeatured", isFeatured.Value);
            }

            if (!string.IsNullOrEmpty(userId))
            {
                filter &= filterBuilder.Eq("user_id", userId);
            }

            var sortDefinition = GetSortDefinition(sortBy, isAscending);
            var blogs = await _collection.Find(filter)
                .Sort(sortDefinition)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var totalBlogs = await GetBlogsCount(filter);

            var userTasks = blogs.Select(b => _userService.GetUserById(b.UserId)).ToList();
            var userResults = await Task.WhenAll(userTasks);
            var blogDtos = blogs.Select((blog, index) => new BlogReadDTO
            {
                Id = blog.Id!,
                Title = blog.Title,
                Content = blog.Content,
                ImageUrls = blog.ImageUrls,
                Category = blog.Category,
                DateCreated = blog.DateCreated,
                ViewCount = blog.ViewCount,
                IsFeatured = blog.IsFeatured,
                FullName = userResults[index]?.FullName ?? "Unknown User"
            });

            return (blogDtos, totalBlogs);
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
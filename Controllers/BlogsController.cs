using Microsoft.AspNetCore.Mvc;
using nova_mas_blog_api.Services;
using nova_mas_blog_api.Models;
using nova_mas_blog_api.DTOs.BlogDTOs;
using nova_mas_blog_api.Enums;

[Route("api/[controller]")]
[ApiController]
public class BlogsController : ControllerBase
{
    private readonly BlogService _blogService;
    private readonly UserService _userService;

    public BlogsController(BlogService blogService, UserService userService)
    {
        _blogService = blogService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogs(int page = 1, int pageSize = 10)
    {
        var blogs = await _blogService.GetAll(page, pageSize);
        return Ok(blogs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlog(string id)
    {
        var blog = await _blogService.GetById(id);
        if (blog == null) return NotFound();

        var user = await _userService.GetById(blog.user_id);
        string fullName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown User";

        var blogDto = new BlogReadDTO
        {
            Id = blog.Id!,
            Title = blog.Title,
            Content = blog.Content,
            ImageUrls = blog.ImageUrls,
            VideoUrls = blog.VideoUrls,
            Category = blog.Category,
            DateCreated = blog.DateCreated,
            ViewCount = blog.ViewCount,
            IsFeatured = blog.IsFeatured,
            FullName = fullName
        };

        return Ok(blogDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlog([FromBody] BlogCreateDTO blogDto)
    {
        var blog = new Blog
        {
            Title = blogDto.Title,
            Content = blogDto.Content,
            ImageUrls = blogDto.ImageUrls,
            VideoUrls = blogDto.VideoUrls,
            Category = blogDto.Category,
            user_id = blogDto.UserId,
            DateCreated = DateTime.UtcNow,
            ViewCount = 0,
            IsFeatured = false
        };
        await _blogService.Create(blog);
        return CreatedAtAction(nameof(GetBlog), new { id = blog.Id }, blog);
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlog(string id, [FromBody] BlogUpdateDTO blogDto)
    {
        var blog = await _blogService.GetById(id);
        if (blog == null) return NotFound();

        blog.Title = blogDto.Title;
        blog.Content = blogDto.Content;
        blog.ImageUrls = blogDto.ImageUrls;
        blog.VideoUrls = blogDto.VideoUrls;
        blog.Category = blogDto.Category;

        await _blogService.Update(id, blog);
        return Ok(blog);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlog(string id)
    {
        var success = await _blogService.Delete(id);
        if (!success) return NotFound();
        return NoContent();
    }

    // ! ||--------------------------------------------------------------------------------||
    // ! ||                                  Fancy Search                                  ||
    // ! ||--------------------------------------------------------------------------------||

    [HttpGet("search")]
    public async Task<IActionResult> SearchBlogs(
    [FromQuery] string searchText = null!,
    [FromQuery] BlogCategory? category = null,
    [FromQuery] bool? isFeatured = null,
    [FromQuery] string sortBy = "date",
    [FromQuery] bool isAscending = true,
    [FromQuery] string userId = null!,
    [FromQuery] string nameSearch = null!,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
    {
        var blogs = await _blogService.SearchBlogs(searchText, category, isFeatured, sortBy, isAscending, userId, nameSearch, page, pageSize);
        return Ok(blogs);
    }



}

using Microsoft.AspNetCore.Mvc;

namespace nova_mas_blog_api.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            // return Content("Welcome to the Nova Mas Blog API. Use /swagger to access the API documentation.");
            return Redirect("/swagger");
        }
    }
}

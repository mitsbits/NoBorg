using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("v1/Posts")]
    [Authorize]
    public class PostsController : Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult Posts(string categories)
        {
            var u = User;
            var posts = new[]
            {
                new {title = $"title {categories}", content = "content 1", link = "http://www.google.com"},
                new {title = $"title {categories}", content = "content 1", link = "http://www.google.com"},
                new {title = $"title {categories}", content = "content 1", link = "http://www.google.com"},
                new {title = $"title {categories}", content = "content 1", link = "http://www.google.com"},
                new {title = $"title {categories}", content = "content 1", link = "http://www.google.com"},
                new {title = $"title {categories}", content = "content 1", link = "http://www.google.com"},
                new {title = $"title {categories}", content = "content 1", link = "http://www.google.com"},
                new {title = $"title {categories}", content = "content 1", link = "http://www.google.com"},
                new {title = $"title {categories}", content = "content 1", link = "http://www.google.com"},
                new {title = $"title {categories}", content = "content 1", link = "http://www.google.com"},
            };
            return Ok(posts);
        }
    }
}
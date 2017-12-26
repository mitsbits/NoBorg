using Borg.Infra.Services.Slugs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("v1/slug")]
    [Authorize]
    public class SlugifyController : Controller
    {
        private readonly ISlugifierService _slugifier;

        public SlugifyController(ISlugifierService slugifier)
        {
            _slugifier = slugifier;
        }

        [HttpGet("{input?}")]
        public IActionResult Slug(string input)
        {
            return Ok(string.IsNullOrWhiteSpace(input) ? string.Empty : _slugifier.Slugify(input, 126));
        }
    }
}
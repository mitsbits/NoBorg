using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.MVC.Services.Slugs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("v1/slug")][Authorize]
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
            if (string.IsNullOrWhiteSpace(input)) return Ok(string.Empty);
            return Ok(_slugifier.Slugify(input, 126));
        }
    }
}

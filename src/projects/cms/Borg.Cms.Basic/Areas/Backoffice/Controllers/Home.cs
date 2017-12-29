using Borg.MVC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Borg.Cms.Basic.Areas.Backoffice.Controllers
{
    [Area("Backoffice")]
    [Authorize]
    public class HomeController : BorgController
    {
        // GET: /<controller>/

        public HomeController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public IActionResult Home()
        {
            SetPageTitle("backoffice home", "hey dude!");
            return View();
        }
    }
}
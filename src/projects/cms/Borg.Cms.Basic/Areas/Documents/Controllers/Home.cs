using Borg.MVC;
using Borg.MVC.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Areas.Documents.Controllers
{
    [Area("Documents")]
    [ControllerTheme("backoffice")]
    public class HomeController : BorgController
    {
        public IActionResult Home()
        {
            return View();
        }

        public HomeController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }
}
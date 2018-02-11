using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.PlugIns.BlogEngine.Areas.BlogEngine.Controllers
{
    public class HomeController : BlogEngineController
    {
        public HomeController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public IActionResult Home()
        {
            return View();
        }
    }
}
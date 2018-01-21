using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.Controllers
{
    public class HomeController : DocumentsController
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
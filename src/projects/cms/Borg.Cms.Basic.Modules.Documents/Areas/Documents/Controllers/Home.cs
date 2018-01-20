using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Modules.Documents.Area.Documents.Controllers
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
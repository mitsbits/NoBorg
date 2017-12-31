using Microsoft.AspNetCore.Mvc;

namespace Borg.Cms.Basic.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
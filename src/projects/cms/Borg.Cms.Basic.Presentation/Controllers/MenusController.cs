using Borg.MVC.BuildingBlocks;
using Borg.MVC.Conventions;
using Microsoft.AspNetCore.Mvc;

namespace Borg.Cms.Basic.Presentation.Controllers
{
    [ControllerTheme("Bootstrap3")]
    public class MenusController : PresentationController
    {
        public IActionResult Root(string rootmenu)
        {
            
           SetPageTitle(rootmenu);
            return View();
        }

        public IActionResult Leaf(string parentmenu, string childmenu)
        {
            SetPageTitle(parentmenu + "/" + childmenu);
            return View();
        }
    }
}
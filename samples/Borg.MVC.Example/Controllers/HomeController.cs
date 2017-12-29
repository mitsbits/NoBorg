using Borg.Infra;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.Extensions;
using Borg.MVC.Services.UserSession;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Borg.Infra.Services.Slugs;

namespace Borg.Mvc.Example.Controllers
{
    public class HomeController : FrameworkController
    {
        public IActionResult Index([FromServices] IContextAwareUserSession usersession, [FromServices] ISlugifierService slugifier)
        {
            SetPageTitle(slugifier.Slugify("Οι σημαντικότερες ειδήσεις της ημέρας."));
            usersession.TryContextualize(this);
            usersession.Push(new ControllerServerResponse(ResponseStatus.Info, "Hallo"));
            usersession.Push(new ControllerServerResponse(ResponseStatus.Critical, "Help"));
     
            return View();
        }

        [TypeFilter(typeof(DeviceLayoutFilter), Arguments = new object[]{"_BlackLayout"} )]
        public async Task< IActionResult> About([FromServices] IContextAwareUserSession usersession)
        {
            usersession.TryContextualize(this);
            var m = usersession.Messages;
            var bytes = new byte[] { 1, 25, 124, 66, 55, 1 };
            var saved = await usersession.SaveFile("object.json", new MemoryStream(bytes));
            var retrived = await usersession.GetFileInfo("object.json");
            var exitsts = await usersession.Exists("object.json");
            var deleted = await usersession.DeleteFile("object.json");
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }

    public class ControllerServerResponse : ServerResponse
    {
        public ControllerServerResponse(ResponseStatus status, string title, string message = "") : base(status, title, message) { }
    }
}

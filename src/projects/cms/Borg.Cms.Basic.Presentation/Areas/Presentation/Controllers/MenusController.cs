using Borg.Cms.Basic.Presentation.Queries;
using Borg.MVC.Conventions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Presentation.Areas.Presentation.Controllers
{
    [ControllerTheme("Bootstrap3")]
    public class MenusController : PresentationController
    {
        public async Task<IActionResult> Root(string rootmenu)
        {
            var result = await Dispatcher.Send(new MenuRootPageContentRequest(rootmenu));
            PageContent(result.Payload);
            return View();
        }

        public IActionResult Leaf(string parentmenu, string childmenu)
        {
            SetPageTitle(parentmenu + "/" + childmenu);
            return View();
        }

        public MenusController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
        {
        }
    }
}
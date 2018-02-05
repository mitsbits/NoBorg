using Borg.Cms.Basic.PlugIns.Documents.Commands;
using Borg.Cms.Basic.PlugIns.Documents.Queries;
using Borg.MVC.Conventions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.Controllers
{
    [ControllerTheme("Backoffice")]
    public class HomeController : DocumentsController
    {
        private readonly IMediator _dispatcher;

        public HomeController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory)
        {
            _dispatcher = dispatcher;
        }

        public IActionResult Home()
        {
            SetPageTitle("docs home");
            return View();
        }

        public async Task<IActionResult> Item(int id)
        {
            var result = await _dispatcher.Send(new DocumentRequest(id));
            if (!result.Succeded)
            {
                return View("404");
            }
            var model = result.Payload;
            SetPageTitle(model.Asset.Name, $"v: {model.Asset.CurrentFile.Version} mt: {model.Asset.CurrentFile.FileSpec.MimeType}");
            return View(result.Payload);
        }

        [HttpPost("[area]/[controller]/ToggleState")]
        public async Task<IActionResult> ToggleState(ToggleStateModel model, string redirecturl)
        {
            if (model.operation == "deleted")
            {
                var result = await _dispatcher.Send(new ToggleDocumentDeletedStateCommand(model.id));
                if (!result.Succeded) AddErrors(result);
            }
            if (model.operation == "published")
            {
                var result = await _dispatcher.Send(new ToggleDocumentPublishedStateCommand(model.id));
                if (!result.Succeded) AddErrors(result);
            }

            return RedirectToLocal(redirecturl);
        }

        [HttpPost("[area]/[controller]/CheckOut")]
        public async Task<IActionResult> CheckOut(int documentId, string redirecturl)
        {
            var command = new CheckOutCommand(documentId, User.Identity.Name);
            var result = await _dispatcher.Send(command);
            if (!result.Succeded)
            {
                AddErrors(result);
            }
            return RedirectToLocal(redirecturl);
        }

        [HttpPost("[area]/[controller]/CheckIn")]
        public async Task<IActionResult> CheckIn(CheckInCommand model, string redirecturl)
        {
     
            var result = await _dispatcher.Send(model);
            if (!result.Succeded)
            {
                AddErrors(result);
            }
            return RedirectToLocal(redirecturl);
        }


    }
    public class CheckInViewModel { }
}
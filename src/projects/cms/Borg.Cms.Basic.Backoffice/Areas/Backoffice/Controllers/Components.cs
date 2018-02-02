using Borg.Cms.Basic.Backoffice.Areas.Backoffice.Components;
using Borg.Cms.Basic.Lib.Features.CMS.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    [Route("[area]/Components")]
    public class ComponentsController : BackofficeController
    {
        public ComponentsController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
        {
        }

        [HttpPost("ComponentDevice")]
        public async Task<IActionResult> ComponentDevice(ComponentDeviceViewModel model, string redirecturl)
        {
            var command = new ComponentDeviceCommand(model.ComponentId, int.Parse(model.DeviceId.ValueModel()[0].Item2));
            var result = await Dispatcher.Send(command);
            if (!result.Succeded)
            {
                AddErrors(result);
            }
            return RedirectToLocal(redirecturl);
        }

        [HttpPost("ToggleState")]
        public async Task<IActionResult> ToggleState(ToggleStateModel model, string redirecturl)
        {
            if (model.operation == "deleted")
            {
                var result = await Dispatcher.Send(new ToggleComponentDeletedStateCommand(model.id));
                if (!result.Succeded) AddErrors(result);
            }
            if (model.operation == "published")
            {
                var result = await Dispatcher.Send(new ToggleComponentPublishedStateCommand(model.id));
                if (!result.Succeded) AddErrors(result);
            }

            return RedirectToLocal(redirecturl);
        }

        public class ToggleStateModel
        {
            public string operation { get; set; }
            public int id { get; set; }
        }
    }
}
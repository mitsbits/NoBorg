using Borg.MVC.PlugIns.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    [Route("[area]/plugins")]
    public class PlugInsController : BackofficeController
    {
        private readonly IPlugInHost _plugInHost;

        public PlugInsController(ILoggerFactory loggerFactory, IMediator dispatcher, IPlugInHost plugInHost) : base(loggerFactory, dispatcher)
        {
            _plugInHost = plugInHost;
        }

        [HttpGet("")]
        public IActionResult Home()
        {
            return View(_plugInHost.PlugIns);
        }
    }
}
using Borg.MVC;
using Borg.MVC.Conventions;
using Borg.MVC.PlugIns.Decoration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    [PlugInEntryPointController("Backoffice")]
    [Authorize]
    [ControllerTheme("Backoffice")]
    public abstract class BackofficeController : BorgController
    {
        protected readonly IMediator Dispatcher;

        protected BackofficeController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory)
        {
            Dispatcher = dispatcher;
        }
    }
}
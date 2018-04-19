using Borg.Bookstore.Features.Users.Policies;
using Borg.MVC;
using Borg.MVC.Conventions;
using Borg.MVC.PlugIns.Decoration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Borg.Bookstore.Areas.Backoffice.Controllers
{
    [PlugInEntryPointController("Backoffice")]
    [Authorize(policy:BackofficePolicies.Backoffice)]
    //[ControllerTheme("Backoffice")]
    public abstract class BackofficeController : BorgController
    {
        protected readonly IMediator Dispatcher;

        protected BackofficeController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory)
        {
            Dispatcher = dispatcher;
        }

    }
}
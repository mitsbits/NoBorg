using Borg.MVC;
using Borg.MVC.Conventions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    [Area("Backoffice")]
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
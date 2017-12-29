using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.Navigation.Queries;
using Borg.MVC;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Areas.Backoffice.Controllers
{
    [Route("[area]/Menus")]
    [Area("Backoffice")]
    [Authorize]
    public class MenusController : BorgController
    {
        private readonly IMediator _dispatcher;
        public MenusController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory)
        {
            _dispatcher = dispatcher;
        }

        public async Task< IActionResult> Home()
        {
            SetPageTitle("Navigational Menus");
            var result = await _dispatcher.Send(new MenuGroupsRequest());
            return View();
        }
    }
}

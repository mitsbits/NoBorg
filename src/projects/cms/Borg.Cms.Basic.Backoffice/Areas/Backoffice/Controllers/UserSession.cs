using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Borg.Cms.Basic.Backoffice.Areas.Backoffice.Components;
using Borg.Cms.Basic.Lib;
using Borg.MVC.Extensions;
using Borg.MVC.Services.UserSession;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    public class UserSessionController : BackofficeController
    {
        private readonly IContextAwareUserSession _userSession;
        public UserSessionController(ILoggerFactory loggerFactory, IMediator dispatcher, IContextAwareUserSession userSession) : base(loggerFactory, dispatcher)
        {
            _userSession = userSession;
        }

        [HttpPost]
        public IActionResult SessionSettings(UserSessionViewModel model, string redirectUrl)
        {
            _userSession.TryContextualize(this);
            _userSession.MenuIsCollapsed(model.MenuIsCollapsed);
            _userSession.RowsPerPage(model.RowsPerPage);
            return Redirect(redirectUrl);
        }
    }

}

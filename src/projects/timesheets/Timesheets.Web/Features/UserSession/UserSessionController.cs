using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.MVC.Services.UserSession;
using Microsoft.AspNetCore.Mvc;
using Timesheets.Web.Infrastructure;

namespace Web.Features.UserSession
{
    [Route("UserSession")]
    public class UserSessionController : Controller
    {
        private readonly IUserSession _userSession;

        public UserSessionController(IUserSession userSession)
        {
            _userSession = userSession;
        }
        [Route("Settings")][HttpPost]
        public IActionResult SessionSettings(UserSessionViewModel model)
        {
            _userSession.MenuIsCollapsed(model.MenuIsCollapsed);
            _userSession.RowsPerPage(model.RowsPerPage);
            return Redirect(model.RedirectUrl);
        }
    }
}
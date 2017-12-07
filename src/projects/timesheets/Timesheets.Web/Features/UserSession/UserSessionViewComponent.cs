using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.MVC.Services.UserSession;
using Timesheets.Web.Infrastructure;

namespace Web.Features.UserSession
{
    public class UserSessionViewComponent : ViewComponent
    {
        private readonly IUserSession _userSession;

        public UserSessionViewComponent(IUserSession userSession)
        {
            _userSession = userSession;
        }

        public  IViewComponentResult Invoke(string view = "")
        {
            var model = new UserSessionViewModel()
            {
                UserIdentifier = _userSession.UserIdentifier,
                UserName = _userSession.UserName,
                SessionStart = _userSession.SessionStart,
                MenuIsCollapsed = _userSession.MenuIsCollapsed(),
                RowsPerPage = _userSession.RowsPerPage()
            };
            return string.IsNullOrWhiteSpace(view) ? View(model) : View(view, model);
        }
    }
}

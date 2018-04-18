using Borg.Bookstore.Areas.Backoffice.ViewComponents;

using Borg.MVC.Extensions;
using Borg.MVC.Services.UserSession;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Bookstore.Areas.Backoffice.Controllers
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
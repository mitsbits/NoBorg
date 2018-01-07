using Borg.Infra;
using Borg.Infra.DTO;
using Borg.MVC;
using Borg.MVC.Extensions;
using Borg.MVC.Services.UserSession;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Borg.Cms.Basic.Areas.Backoffice.Controllers
{
    [Area("Backoffice")]
    [Authorize]
    public class HomeController : BorgController
    {
        // GET: /<controller>/

        public HomeController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public IActionResult Home([FromServices]IContextAwareUserSession session)
        {
            SetPageTitle("backoffice home", "hey dude!");
            session.TryContextualize(this);
            session.Push(new ServerResponse(ResponseStatus.Info, "Hi there", "this is a message"));
            session.Push(new ServerResponse(ResponseStatus.Error, "Hi there", "this is a message"));
            session.Push(new ServerResponse(ResponseStatus.Success, "Hi there", "this is a message"));
            session.Push(new ServerResponse(ResponseStatus.Undefined, "Hi there", "this is a message"));
            return View();
        }
    }
}
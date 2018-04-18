using System.Threading.Tasks;
using Borg.Infra;
using Borg.Infra.Caching.Contracts;
using Borg.Infra.DTO;
using Borg.MVC.Extensions;
using Borg.MVC.Services.UserSession;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Borg.Bookstore.Areas.Backoffice.Controllers
{
    public class HomeController : BackofficeController
    {
        private readonly ICacheStore _cache;

        public HomeController(ILoggerFactory loggerFactory, IMediator dispatcher, ICacheStore cache) : base(loggerFactory, dispatcher)
        {
            _cache = cache;
        }

        public async Task<IActionResult> Home([FromServices]IContextAwareUserSession session)
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
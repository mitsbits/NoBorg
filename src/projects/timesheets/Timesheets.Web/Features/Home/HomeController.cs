using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Timesheets.Web.Domain;
using Timesheets.Web.Infrastructure;

namespace Timesheets.Web.Features.Home
{
    [Route("")]
    [Authorize]
    public class HomeController : FrameworkController
    {


        public HomeController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {

        }


        public IActionResult Home()
        {
            SetPageTitle("Timesheets bro");
            Logger.LogInformation("this is the {@controler}", this.GetType());
            return View();
        }
    }
}

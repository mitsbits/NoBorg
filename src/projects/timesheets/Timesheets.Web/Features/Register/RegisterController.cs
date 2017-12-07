using System.Threading.Tasks;
using Borg.MVC.BuildingBlocks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Timesheets.Web.Domain.Infrastructure;
using Timesheets.Web.Features.Home;
using Timesheets.Web.Infrastructure;
using Borg.Infra.DAL;

namespace Timesheets.Web.Features.Register
{
    [Route("Register")]
    [TypeFilter(typeof(DeviceLayoutFilter), Arguments = new object[] { "_EmptyLayout" })]
    public class RegisterController : FrameworkController
    {

        private readonly IMediator _dispatcher;
        public RegisterController(ILoggerFactory loggerFactory,  IMediator dispatcher) : base(loggerFactory)
        {
     
            _dispatcher = dispatcher;
        }

        [Route("")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register(string returnurl)
        {
            this.SetPageTitle("Register new user");
            return View(new RegisterViewModel() { ReturnUrl = returnurl });
        }

        [Route("")]
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnurl)
        {
            if (ModelState.IsValid)
            {

                var commadresult = await _dispatcher.Send(new CreateWorkerCommand(model));
                if (commadresult.Succeded)
                {
                    SetPageTitle("Successful Registration");
                    return View("RegistrationSuccess", model);
                }
                else
                {

                    AddErrors(commadresult);
                }

            }
            return View(model);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private void AddErrors(CommandResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Home), "Home");
            }
        }
    }
}
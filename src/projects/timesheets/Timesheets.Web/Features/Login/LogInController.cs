using System.Threading.Tasks;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Timesheets.Web.Auth;
using Timesheets.Web.Features.Home;
using Timesheets.Web.Infrastructure;

namespace Timesheets.Web.Features.Login
{
    [Route("Login")]
    [TypeFilter(typeof(DeviceLayoutFilter), Arguments = new object[] { "_EmptyLayout" })]
    public class LoginController : FrameworkController
    {

        private readonly SignInManager<ApplicationUser> _signInManager;


        public LoginController(ILoggerFactory loggerFactory,  SignInManager<ApplicationUser> signInManager) : base(loggerFactory)
        {

            _signInManager = signInManager;

        }
      
        [Route("")]
        [HttpGet]
        [AllowAnonymous]
        public  IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            SetPageTitle("Log In");
            return View(new LoginViewModel{ReturnUrl = returnUrl});
        }
        [Route("")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    Logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    Logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken][Route("out")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            Logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Home), "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(HomeController.Home), "Home");
        }
    }
}
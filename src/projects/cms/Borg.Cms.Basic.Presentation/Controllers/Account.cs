using Borg.Cms.Basic.Lib.Features.Auth;
using Borg.Cms.Basic.Lib.Features.Auth.Data;
using Borg.Cms.Basic.Lib.Features.Auth.Events;
using Borg.Cms.Basic.Lib.Features.Auth.Login;
using Borg.Cms.Basic.Lib.Features.Auth.Register;
using Borg.Infra;
using Borg.Infra.DAL;
using Borg.MVC;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.Conventions;
using Borg.MVC.Services.UserSession;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Presentation.Controllers
{
    [Route("")]
    [ControllerTheme("Bootstrap3")]
    [TypeFilter(typeof(DeviceLayoutFilter), Arguments = new object[] { "_EmptyLayout" })]
    public class AccountController : BorgController
    {
        private readonly ILogger _logger;
        private readonly IMediator _dispatcher;
        private readonly SignInManager<CmsUser> _signInManager;
        private readonly UserManager<CmsUser> _userManager;
        private readonly IUnitOfWork<AuthDbContext> _uow;
        private readonly BorgSettings _settings;

        public AccountController(ILoggerFactory loggerFactory, IMediator dispatcher, SignInManager<CmsUser> signInManager, UserManager<CmsUser> userManager, IUnitOfWork<AuthDbContext> uow, BorgSettings settings)
            : base(loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _dispatcher = dispatcher;
            _signInManager = signInManager;
            _userManager = userManager;
            _uow = uow;
            _settings = settings;
        }

        [Route("Login")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            SetPageTitle("Log In");
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromServices]IContextAwareUserSession session, LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid user.");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.Info("{user} logged in.", model.Email);

                    return RedirectToLocal(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    Logger.Warn("{user} account locked out.", model.Email);
                    return View("Lockout", model);
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout([FromServices] IContextAwareUserSession userSession)
        {
            var name = User.Identity.Name;
            await _signInManager.SignOutAsync();
            userSession.StopSession();
            //Logger.Info("{User} logged out.", name);
            return Redirect(Url.Content("~/"));
        }

        [Route("Denied")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Denied(string returnurl)
        {
            var notification = new AccessDeniedToUser(User?.Identity?.Name, returnurl);
            _dispatcher.Publish(notification);
            SetPageTitle("Access denied");
            return View(notification);
        }

        [Route("Register")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            SetPageTitle("Register new user");
            return View(new RegistrationRequestCommand());
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegistrationRequestCommand model, string returnurl)
        {
            if (!_settings.Auth.ActivateOnRegisterRequest)
            {
                if (ModelState.IsValid)
                {
                    var commadresult = await _dispatcher.Send(model);
                    if (commadresult.Succeded)
                    {
                        var hit = commadresult.Payload.CompositeKey;

                        await _dispatcher.Publish(new RegistrationRequestEvent(hit));
                        return RedirectToAction("RegistrationVerification", "Account", new { email = hit.Partition });
                    }
                    AddErrors(commadresult);
                }
                return View(model);
            }
            {
                var commadresult = await _dispatcher.Send(model);
                if (commadresult.Succeded) return RedirectToLocal(returnurl);

                AddErrors(commadresult);
                return View(model);
            }
        }

        [AllowAnonymous]
        [HttpGet("RegistrationVerification/{email}")]
        public async Task<IActionResult> RegistrationVerification(string email)
        {
            var repo = _uow.QueryRepo<RegistrationRequest>();

            var hits = await repo.Find(request => request.Email == email,
                new[] { new OrderByInfo<RegistrationRequest>(x => x.SubmitedOn, false), });
            if (!hits.Any()) return NotFound(email);

            var hit = hits.OrderByDescending(x => x.SubmitedOn).First();

            SetPageTitle("Registration Verification", hit.Email);

            var model = new RegisterViewModel() { Email = hit.Email };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost("RegistrationVerification")]
        public async Task<IActionResult> RegistrationVerification(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var commadresult = await _dispatcher.Send(new RegisterCommand(model));
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
    }
}
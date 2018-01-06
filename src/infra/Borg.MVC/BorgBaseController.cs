using Microsoft.AspNetCore.Mvc;

namespace Borg.MVC
{
    public abstract class BorgBaseController : Controller
    {
        [NonAction]
        protected IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect(Url.Content("~/"));
        }
    }
}
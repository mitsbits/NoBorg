using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timesheets.Web.Infrastructure;

namespace Timesheets.Web.ViewComponents
{
    public class MainHeaderNavBarViewComponent : ViewComponent
    {
        private readonly IApplicationUserSession _session;

        public MainHeaderNavBarViewComponent(IApplicationUserSession session)
        {
            _session = session;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var claims = User.Identity as ClaimsIdentity;
            //var a = claims.FindFirst(x => x.Type == BorgSpecificClaims.Profile.Avatar).Value;
            //var n = claims.Name;
            //var roles = claims.Claims.Where(c => c.Type == claims.RoleClaimType).Select(x => x.Value);
            //var user = new SidebarUserInfoViewModel() { Nickname = n, Avatar = a, Roles = roles?.ToArray(), Id = claims.GetSubjectId() };
            //var pending = await _userNotifications.Pending(User.GetSubjectId(), 1, 10);
            //user.UserNotifications = pending?.ToArray();
            return View(new MainHeaderNavBarViewModel());
        }
    }

    public class MainHeaderNavBarViewModel
    {

    }
}
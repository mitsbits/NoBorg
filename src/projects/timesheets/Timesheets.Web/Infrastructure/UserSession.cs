using Borg.Infra;
using Borg.Infra.Storage;
using Borg.MVC.Services.ServerResponses;
using Borg.MVC.Services.UserSession;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Timesheets.Web.Auth;

namespace Timesheets.Web.Infrastructure
{
    public partial class UserSession
    {
        public const string SettingsCookieName = "Borg.UserSession";
        public const string SessionStartKey = "SessionStartKey";
        public const string MenuIsCollapsed = "Borg.MenuIsCollapsed";
        public const string RowsPerPage = "Borg.RowsPerPage";
    }

    public partial class UserSession : BorgUserSession, IUserSession, IApplicationUserSession
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSession(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, ISerializer serializer, ISessionServerResponseProvider provider, IFileStorage storage) : base(httpContextAccessor, serializer, provider, storage)
        {
            _userManager = userManager;
        }

        public string Avatar
            =>
            IsAuthenticated() && HttpContext.User.Claims.Any(x => x.Type == "Avatar")
                ? HttpContext.User.Claims.First(x => x.Type == "Avatar").Value
                : @"https://cdn2.iconfinder.com/data/icons/ios-7-icons/50/user_male2-512.png";

        public bool IsInRole(Roles role)
        {
            return IsAuthenticated() && HttpContext.User.IsInRole(role.ToString());
        }
    }
}
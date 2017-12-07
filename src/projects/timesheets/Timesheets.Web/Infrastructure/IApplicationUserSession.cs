using Borg.MVC.Services.UserSession;
using Timesheets.Web.Auth;

namespace Timesheets.Web.Infrastructure
{
    public interface IApplicationUserSession : IUserSession, IContextAwareUserSession
    {
        bool IsInRole(Roles role);
        string Avatar { get; }
    }



}

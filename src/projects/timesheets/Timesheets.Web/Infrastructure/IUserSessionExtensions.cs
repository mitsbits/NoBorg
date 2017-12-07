using System;
using Borg.MVC.Services.UserSession;
using Timesheets.Web.Auth;
using UserSession = Timesheets.Web.Infrastructure.UserSession;

namespace Timesheets.Web.Infrastructure
{
    public static class IUserSessionExtensions
    {
        
        public static bool MenuIsCollapsed(this IUserSession userSession)
        {
            return userSession.Setting<bool>(UserSession.MenuIsCollapsed);
        }
        public static bool MenuIsCollapsed(this IUserSession userSession, bool value)
        {
            return userSession.Setting(UserSession.MenuIsCollapsed, value);
        }

        public static int RowsPerPage(this IUserSession userSession)
        {
            var result = 10;
            try
            {
                result = userSession.Setting<int>(UserSession.RowsPerPage);
                if (result < 1) result = 10;
            }
            catch (Exception )
            {
                result = userSession.Setting(UserSession.RowsPerPage, result);
            }
            return result;
        }
        public static int RowsPerPage(this IUserSession userSession, int value)
        {
            return userSession.Setting(UserSession.RowsPerPage, value);
        }

        public static bool IsAdmin(this IApplicationUserSession userSession)
        {
            return userSession.IsInRole(Roles.Admin);
        }

        public static bool IsManager(this IApplicationUserSession userSession)
        {
            return userSession.IsInRole(Roles.Manager);
        }

        public static bool IsAdminOrManager(this IApplicationUserSession userSession)
        {
            return userSession.IsAdmin() || userSession.IsManager();
        }
    }


}
using Borg.MVC.Services.UserSession;
using System;

namespace Borg.Cms.Basic.Lib
{
    public static class IUserSessionExtensions
    {
        private const string MenuIsCollapsedKey = "MenuIsCollapsed";
        private const string MenuExpandOnHoverKey = "MenuExpandOnHover";
        private const string RowsPerPageKey = "RowsPerPage";
        private const string AvatarKey = "Avatar";


        public static bool MenuIsCollapsed(this IUserSession userSession)
        {
            return userSession.Setting<bool>(MenuIsCollapsedKey);
        }

        public static bool MenuIsCollapsed(this IUserSession userSession, bool value)
        {
            return userSession.Setting(MenuIsCollapsedKey, value);
        }

        public static bool MenuExpandOnHover(this IUserSession userSession)
        {
            return userSession.Setting<bool>(MenuExpandOnHoverKey);
        }

        public static bool MenuExpandOnHover(this IUserSession userSession, bool value)
        {
            return userSession.Setting(MenuExpandOnHoverKey, value);
        }

        public static int RowsPerPage(this IUserSession userSession)
        {
            var result = 10;
            try
            {
                result = userSession.Setting<int>(RowsPerPageKey);
                if (result < 1) result = 10;
            }
            catch (Exception)
            {
                result = userSession.Setting(RowsPerPageKey, result);
            }
            return result;
        }

        public static int RowsPerPage(this IUserSession userSession, int value)
        {
            return userSession.Setting(RowsPerPageKey, value);
        }

        public static string Avatar(this IUserSession userSession)
        {
            return userSession.Setting<string>(AvatarKey);
        }

        public static string Avatar(this IUserSession userSession, string value)
        {
            return userSession.Setting(AvatarKey, value);
        }
    }
}
using System.Linq;
using System.Security.Claims;

namespace Borg
{
    public static class ClaimsIdentityExtensions
    {
        public static bool IsInAnyRole(this ClaimsPrincipal principal, params string[] roles)
        {
            return roles.Any(principal.IsInRole);
        }
    }
}
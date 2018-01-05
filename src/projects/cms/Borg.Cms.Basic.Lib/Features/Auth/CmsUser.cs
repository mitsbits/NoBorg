using Microsoft.AspNetCore.Identity;

namespace Borg.Cms.Basic.Lib.Features.Auth
{
    public class CmsUser : IdentityUser
    {
    }

    public class CmsRole : IdentityRole
    {
        public CmsRole(string roleName) : base(roleName)
        {
        }
    }
}
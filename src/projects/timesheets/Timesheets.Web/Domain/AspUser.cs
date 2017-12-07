using System;
using Timesheets.Web.Domain.Infrastructure;

namespace Timesheets.Web.Domain
{
    public class AspUser : Entity<string>
    {
        public string Email { get; set; }
        public bool LockoutEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
    }

    public class AspRole : Entity<string>
    {
        public string Name { get; set; }

    }

    public class AspUserRole
    {
 
        public string UserId { get; set; }


        public string RoleId { get; set; }
    }
}

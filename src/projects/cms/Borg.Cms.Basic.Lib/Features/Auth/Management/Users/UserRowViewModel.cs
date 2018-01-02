using System;

namespace Borg.Cms.Basic.Lib.Features.Auth.Management.Users
{
    public class UserRowViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool LockedOut { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] Roles { get; set; }
        public DateTimeOffset? LockedOutEnd { get; set; }

        public string DisplayName => $"{FirstName} {LastName}";
    }
}
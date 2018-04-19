using System;

namespace Borg.Platform.Identity
{
    [Flags]
    public enum SystemRoles
    {
        ReadOnly = 2 ^ 0,
        Writer = 2 ^ 1,
        Developer = 2 ^ 2,
        Admin = 2 ^ 3,
    }
}
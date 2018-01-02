using System;

namespace Borg.Cms.Basic.Lib.Features.Auth
{
    [Flags]
    public enum SystemRoles
    {
        ReadOnly = 2,
        Writer = 4,
        Developer = 8,
        Admin = 16,
    }

    public enum CmsRoles
    {
        Backoffice = 2,
        Author = 4,
        Blogger = 8,
        Editor = 16,
        Manager = 32,
    }
}
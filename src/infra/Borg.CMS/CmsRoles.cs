using System;

namespace Borg.CMS
{
    [Flags]
    public enum CmsRoles
    {
        Backoffice = 2 ^ 0,
        Author = 2 ^ 1,
        Blogger = 2 ^ 2,
        Editor = 2 ^ 3,
        Manager = 2 ^ 4,
    }
}
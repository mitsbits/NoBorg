using System.Linq;
using Borg.Infra.Services;

namespace Borg.Platform.Identity.Configuration
{
    public class AuthDbSeedOptions
    {
        public bool CreateSystemRoles { get; set; } = true;
        public bool CreateDefaultAdmin { get; set; } = true;
        public string DefaultAdminName { get; set; } = "admin@borg.net";
        public string DefaultAdminPassword { get; set; } = "P@ssw0rd";
        public string[] DefaultAdminRoles { get; set; } = EnumUtil.GetValues<SystemRoles>().Where(x => x != SystemRoles.ReadOnly).Select(x => x.ToString()).ToArray();
    }
}
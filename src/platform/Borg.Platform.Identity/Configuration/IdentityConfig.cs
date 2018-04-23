using Borg.Infra.Configuration.Contracts;
using Borg.Platform.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using Borg.Infra.Services;

namespace Borg.Platform.Identity.Configuration
{
    public class IdentityConfig : ISettings
    {
        public string SqlConnectionString { get; set; }
        public bool ActivateOnRegisterRequest { get; set; } = false;
        public string LoginPath { get; set; } = "/login";
        public string LogoutPath { get; set; } = "/logout";
        public string AccessDeniedPath { get; set; } = "/denied";
        public PasswordOptions PasswordOptions { get; set; } = new PasswordOptions();
        public LockoutOptions LockoutOptions { get; set; } = new LockoutOptions();
        public TimeSpan ExpireTimeSpan { get; set; } = TimeSpan.FromDays(15);
        public AuthDbSeedOptions DbSeedOptions { get; set; } = new AuthDbSeedOptions();
    }

    public class AuthDbSeedOptions
    {
        public bool CreateSystemRoles { get; set; } = true;
        public bool CreateDefaultAdmin { get; set; } = true;
        public string DefaultAdminName { get; set; } = "admin@borg.net";
        public string DefaultAdminPassword { get; set; } = "P@ssw0rd";
        public string[] DefaultAdminRoles { get; set; } = EnumUtil.GetValues<SystemRoles>().Where(x => x != SystemRoles.ReadOnly).Select(x => x.ToString()).ToArray();
    }
}
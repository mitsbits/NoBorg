using Borg.Infra.Configuration.Contracts;
using Borg.Platform.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;

namespace Borg.Platform.Identity.Configuration
{
    public class IdentityConfig : ISettings
    {
        public string SqlConnectionString { get; set; }
        public AuthDbSeedOptions DbSeedOptions { get; set; } = new AuthDbSeedOptions();
        public bool ActivateOnRegisterRequest { get; set; } = false;
        public string LoginPath { get; set; } = "/login";
        public string LogoutPath { get; set; } = "/logout";
        public string AccessDeniedPath { get; set; } = "/denied";
        public PasswordOptions PasswordOptions { get; set; } = new PasswordOptions();
        public LockoutOptions LockoutOptions { get; set; } = new LockoutOptions();
        public TimeSpan ExpireTimeSpan { get; set; } = TimeSpan.FromDays(15);
    }
}
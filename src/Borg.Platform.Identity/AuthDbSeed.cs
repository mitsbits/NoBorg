using Borg.Infra.Services;
using Borg.Platform.EF.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Borg.Infra.Configuration.Contracts;

namespace Borg.Platform.Identity
{

    public class IdentityConfig : ISettings
    {
        public string SqlConnectionString { get; set; }
        public AuthDbSeedOptions DbSeedOptions { get; set; } = new AuthDbSeedOptions();
        public bool ActivateOnRegisterRequest { get; set; } = false;
        public string LoginPath { get; set; } = "/login";
        public string LogoutPath { get; set; } = "/logout";
        public string AccessDeniedPath { get; set; } = "/denied";
        public double DaysUntilExpiration { get; set; } = 15;
        public PasswordOptions PasswordOptions { get; set; } = new PasswordOptions();
        public LockoutOptions LockoutOptions { get; set; } = new LockoutOptions();
        public TimeSpan ExpireTimeSpan { get; set; } = TimeSpan.FromDays(15);
    }
    public class AuthDbSeedOptions
    {
        public bool CreateSystemRoles { get; set; } = true;
        public bool CreateDefaultAdmin { get; set; } = true;
        public string DefaultAdminName { get; set; } = "admin@borg.net";
        public string DefaultAdminPassword { get; set; } = "P@ssw0rd";
        public string[] DefaultAdminRoles { get; set; } = EnumUtil.GetValues<SystemRoles>().Where(x => x != SystemRoles.ReadOnly).Select(x => x.ToString()).ToArray();
    }

    public class AuthDbSeed : IDbSeed
    {
        private readonly AuthDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<GenericUser> _userManager;
        private readonly ILogger _logger;
        private readonly AuthDbSeedOptions _options;

        public AuthDbSeed(ILoggerFactory loggerFactory, AuthDbContext db, RoleManager<IdentityRole> roleManager, UserManager<GenericUser> userManager, AuthDbSeedOptions options)
        {
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            _db = db;
            _options = options;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task EnsureUp()
        {
            await _db.Database.MigrateAsync();
            if (_options.CreateSystemRoles) await EnsurPlugInRoles();
            if (_options.CreateDefaultAdmin) await EnsureDefaultUser();
        }

        private async Task EnsurPlugInRoles()
        {
            foreach (var role in EnumUtil.GetValues<SystemRoles>().Select(x => x.ToString()))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(role));
                    if (result.Succeeded)
                    {
                        _logger.Info("Role {role} created", role);
                    }
                    else
                    {
                        _logger.Warn("Failed to create role {role} - reason: {errors}", role, string.Join("|", result.Errors.Select(x => $"{x.Code}-{x.Description}")));
                    }
                }
            }
        }

        private async Task EnsureDefaultUser()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(_options.DefaultAdminName);
                if (user != null)
                {
                    var passwordValid = await _userManager.CheckPasswordAsync(user, _options.DefaultAdminPassword);
                    if (!passwordValid)
                    {
                        _logger.Info("Reseting default user because of passworg mismatch");
                        await _userManager.DeleteAsync(user);
                        user = null;
                    }
                }
                if (user == null)
                {
                    user = new GenericUser() { UserName = _options.DefaultAdminName, Email = _options.DefaultAdminName };
                    await _userManager.CreateAsync(user, _options.DefaultAdminPassword);
                    await _userManager.AddToRolesAsync(user, _options.DefaultAdminRoles);
                    await _userManager.SetLockoutEnabledAsync(user, false);
                    _logger.Info("Created default user {user}", _options.DefaultAdminName);
                }
            }
            catch (Exception e)
            {
                _logger.Warn("Failed to created default user - reason: {reason}", e.ToString());
                _logger.Error(e);
            }
        }
    }
}
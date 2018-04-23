using Borg.Infra.Services;
using Borg.Platform.EF.Contracts;
using Borg.Platform.Identity.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Borg.Infra.Configuration.Contracts;
using Borg.Platform.Identity.Configuration;

namespace Borg.Platform.Identity.Data
{


    public class AuthDbSeed : IDbSeed
    {
        private readonly AuthDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<GenericUser> _userManager;
        private readonly ILogger _logger;
        private readonly ISettingsProvider<IdentityConfig> _options;

        public AuthDbSeed(ILoggerFactory loggerFactory, AuthDbContext db, RoleManager<IdentityRole> roleManager, UserManager<GenericUser> userManager, ISettingsProvider<IdentityConfig> options)
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
            if (_options.Config.DbSeedOptions.CreateSystemRoles) await EnsurPlugInRoles();
            if (_options.Config.DbSeedOptions.CreateDefaultAdmin) await EnsureDefaultUser();
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
                var user = await _userManager.FindByNameAsync(_options.Config.DbSeedOptions.DefaultAdminName);
                if (user != null)
                {
                    var passwordValid = await _userManager.CheckPasswordAsync(user, _options.Config.DbSeedOptions.DefaultAdminPassword);
                    if (!passwordValid)
                    {
                        _logger.Info("Reseting default user because of passworg mismatch");
                        await _userManager.DeleteAsync(user);
                        user = null;
                    }
                }
                if (user == null)
                {
                    user = new GenericUser() { UserName = _options.Config.DbSeedOptions.DefaultAdminName, Email = _options.Config.DbSeedOptions.DefaultAdminName };
                    await _userManager.CreateAsync(user, _options.Config.DbSeedOptions.DefaultAdminPassword);
                    await _userManager.AddToRolesAsync(user, _options.Config.DbSeedOptions.DefaultAdminRoles);
                    await _userManager.SetLockoutEnabledAsync(user, false);
                    _logger.Info("Created default user {user}", _options.Config.DbSeedOptions.DefaultAdminName);
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
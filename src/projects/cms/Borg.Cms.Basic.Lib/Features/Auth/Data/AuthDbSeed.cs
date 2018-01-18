using Borg.MVC;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Borg.Infra;

namespace Borg.Cms.Basic.Lib.Features.Auth.Data
{
    public class AuthDbSeed
    {
        private readonly AuthDbContext _db;
        private readonly BorgSettings _settings;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CmsUser> _userManager;
        private readonly ILogger _logger;

        public AuthDbSeed(ILoggerFactory loggerFactory, AuthDbContext db, BorgSettings settings, RoleManager<IdentityRole> roleManager, UserManager<CmsUser> userManager)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _db = db;
            _settings = settings;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task EnsureUp()
        {
            await _db.Database.MigrateAsync();
            await EnsureSystemRoles();
            await EnsureCmsRoles();
            await EnsureDefaultUser();
        }

        private async Task EnsureSystemRoles()
        {
            foreach (var role in EnumUtil.GetValues<SystemRoles>())
            {
                if (!await _roleManager.RoleExistsAsync($"system.{role.ToString()}"))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole($"system.{role.ToString()}"));
                    if (result.Succeeded)
                    {
                        _logger.Info("Role {role} created", $"system.{role}");
                    }
                    else
                    {
                        _logger.Warn("Failed to create role {role} - reason: {errors}", $"system.{role}", string.Join("|", result.Errors.Select(x => $"{x.Code}-{x.Description}")));
                    }
                }
            }
        }

        private async Task EnsureCmsRoles()
        {
            foreach (var role in EnumUtil.GetValues<CmsRoles>())
            {
                if (!await _roleManager.RoleExistsAsync($"cms.{role.ToString()}"))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole($"cms.{role.ToString()}"));
                    if (result.Succeeded)
                    {
                        _logger.Info("Role {role} created", $"cms.{role}");
                    }
                    else
                    {
                        _logger.Warn("Failed to create role {role} - reason: {errors}", $"cms.{role}", string.Join("|", result.Errors.Select(x => $"{x.Code}-{x.Description}")));
                    }
                }
            }
        }

        private async Task EnsureDefaultUser()
        {
            if (_settings?.Auth?.DefaultUser?.UserName?.Trim().Length > 3)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(_settings.Auth.DefaultUser.UserName);
                    if (user != null)
                    {
                        var passwordValid = await _userManager.CheckPasswordAsync(user, _settings.Auth.DefaultUser.Password);
                        if (!passwordValid)
                        {
                            _logger.Info("Reseting default user because of passworg mismatch");
                            await _userManager.DeleteAsync(user);
                            user = null;
                        }
                    }
                    if (user == null)
                    {
                        user = new CmsUser() { UserName = _settings.Auth.DefaultUser.UserName, Email = _settings.Auth.DefaultUser.Email };
                        await _userManager.CreateAsync(user, _settings.Auth.DefaultUser.Password);
                        await _userManager.AddToRolesAsync(user, _settings.Auth.DefaultUser.Roles);
                        await _userManager.SetLockoutEnabledAsync(user, false);
                        _logger.Info("Created default user {user}", _settings.Auth.DefaultUser.UserName);
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
}
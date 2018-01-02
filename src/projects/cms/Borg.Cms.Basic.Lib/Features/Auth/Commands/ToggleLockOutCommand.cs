using Borg.Infra.DAL;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Auth.Commands
{
    public class ToggleLockOutCommand : CommandBase<CommandResult>
    {
        public ToggleLockOutCommand(string email, DateTime lockOutEnd = default(DateTime))
        {
            Email = email;
            LockOutEnd = lockOutEnd;
        }

        public string Email { get; }
        public DateTime LockOutEnd { get; }
    }

    public class ToggleLockOutCommandHandler : AsyncRequestHandler<ToggleLockOutCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly UserManager<CmsUser> _userManager;

        public ToggleLockOutCommandHandler(ILoggerFactory loggerFactory, UserManager<CmsUser> userManager)
        {
            _logger = loggerFactory.CreateLogger(GetType());

            _userManager = userManager;
        }

        protected override async Task<CommandResult> HandleCore(ToggleLockOutCommand message)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(message.Email);
                if (user == null)
                {
                    _logger.LogDebug(1, "Can not toggle lock, no user : {user}", message.Email);
                    return CommandResult.Failure($"Can not toggle lock, no user : {message.Email}");
                }
                var current = user.LockoutEnabled;
                var identityResult = await _userManager.SetLockoutEnabledAsync(user, !current);

                if (identityResult.Succeeded)
                {
                    if (!current)
                    {
                        var now = DateTimeOffset.UtcNow;
                        var inputUtc = message.LockOutEnd.ToUniversalTime();

                        if (now < inputUtc)
                        {
                            await _userManager.SetLockoutEndDateAsync(user, inputUtc);
                        }
                        else
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(42)));
                        }
                    }

                    _logger.LogDebug(2, "Toggle lock from {current} to {new}, user : {user}", current, !current,
                        message.Email);
                    return CommandResult.Success();
                }
                _logger.LogDebug(3, "Could not toggle lock from {current} to {new}, user : {user}", current, !current, message.Email);
                return CommandResult.Failure(identityResult.Errors.Select(x => x.Description).ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error registering new user: @exception", ex);
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}
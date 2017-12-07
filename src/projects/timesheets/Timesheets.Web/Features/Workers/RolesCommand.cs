using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Timesheets.Web.Auth;
using Timesheets.Web.Domain.Infrastructure;
using Borg.Infra.DAL;

namespace Timesheets.Web.Features.Workers
{
    public class RolesCommand : IRequest<CommandResult>
    {
        public RolesCommand(string email, string[] roles)
        {
            Email = email;
            Roles = roles;
        }
        public string Email { get; }
        public string[] Roles { get; }


    }

    public class RolesCommandHandler : IAsyncRequestHandler<RolesCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly UserManager<ApplicationUser> _userManager;

        public RolesCommandHandler(ILoggerFactory loggerFactory, UserManager<ApplicationUser> userManager)
        {
            _logger = loggerFactory.CreateLogger(typeof(ToggleLockOutCommandHandler));

            _userManager = userManager;

        }

        public async Task<CommandResult> Handle(RolesCommand message)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(message.Email);
                if (user == null)
                {
                    _logger.LogDebug(1, "Can not set roles, no user : {user}", message.Email);
                    return CommandResult.Failure($"Can not set roles, no user : {message.Email}");
                }

                foreach (var r in new[] { Roles.Admin.ToString(), Roles.Manager.ToString() })
                {
                    await _userManager.RemoveFromRoleAsync(user, r);
                }

                foreach (var messageRole in message.Roles)
                {
                    await _userManager.AddToRoleAsync(user, messageRole);
                }

                _logger.LogDebug(2, "Set roles for user : {user}", message.Email);
                return CommandResult.Success();


            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error setting roles for user - @exception", ex);
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}

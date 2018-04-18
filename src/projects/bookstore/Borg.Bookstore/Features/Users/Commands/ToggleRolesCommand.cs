using Borg.Infra.DAL;
using Borg.Platform.Identity.Data;
using Borg.Platform.Identity.Data.Entities;
using Borg.Platform.MediatR;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Bookstore.Features.Users.Commands
{
    public class ToggleRolesCommand : CommandBase<CommandResult>, IRequest<CommandResult>
    {
        public ToggleRolesCommand(string email, params string[] roles)
        {
            Email = email;
            Roles = roles;
        }

        public string Email { get; }
        public string[] Roles { get; }
    }

    public class ToggleRolesCommandHandler : AsyncRequestHandler<ToggleRolesCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly UserManager<GenericUser> _userManager;

        private readonly AuthDbContext _db;

        public ToggleRolesCommandHandler(ILoggerFactory loggerFactory, UserManager<GenericUser> userManager, AuthDbContext db)
        {
            _logger = loggerFactory.CreateLogger(GetType());

            _userManager = userManager;

            _db = db;
        }

        protected override async Task<CommandResult> HandleCore(ToggleRolesCommand message)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(message.Email);
                if (user == null)
                {
                    _logger.LogDebug(1, "Can not set roles, no user : {user}", message.Email);
                    return CommandResult.Failure($"Can not set roles, no user : {message.Email}");
                }

                foreach (var messageRole in message.Roles)
                {
                    if (await _userManager.IsInRoleAsync(user, messageRole))
                    {
                        await _userManager.RemoveFromRoleAsync(user, messageRole);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, messageRole);
                    }
                }

                _logger.LogDebug(2, "Set roles for user : {user}", message.Email);

                var rolesQuery = from r in _db.Roles
                                 join ur in _db.UserRoles on r.Id equals ur.RoleId
                                 join atlasUser in _db.Users on ur.UserId equals atlasUser.Id
                                 orderby r.Name
                                 where atlasUser.Email == message.Email
                                 select r.Name;

                var urls = rolesQuery.ToArray();

                return CommandResult<string[]>.Success(urls);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error setting roles for user - @exception", ex);
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}
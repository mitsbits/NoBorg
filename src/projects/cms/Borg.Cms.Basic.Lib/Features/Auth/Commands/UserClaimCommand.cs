using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Borg.Infra.DAL;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Lib.Features.Auth.Commands
{
    public class UserClaimCommand : CommandBase<CommandResult>
    {
        public UserClaimCommand()
        {
        }

        public UserClaimCommand(string email, string claimType, string claimValue = "")
        {
            Email = email;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        [Required]
        public string Email { get; set; }

        [Required]
        [DisplayName("Claim Type")]
        public string ClaimType { get; set; }

        [Required]
        [DisplayName("Claim Value")]
        public string ClaimValue { get; set; }
    }

    public class UserClaimCommandHandler : AsyncRequestHandler<UserClaimCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly UserManager<CmsUser> _userManager;

        public UserClaimCommandHandler(ILoggerFactory loggerFactory, UserManager<CmsUser> userManager)
        {
            _logger = loggerFactory.CreateLogger(GetType());

            _userManager = userManager;
        }

        protected override async Task<CommandResult> HandleCore(UserClaimCommand message)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(message.Email);
                if (user == null)
                {
                    _logger.Debug("Cannot update avatar, no user : {user}", message.Email);
                    return CommandResult.Failure($"Cannot update avatar, no user : {message.Email}");
                }

                var claims = await _userManager.GetClaimsAsync(user);

                IdentityResult result;

                if (claims.Any(x => x.Type == message.ClaimType))
                {
                    var toremove = claims.Where(x =>
                        x.Type == message.ClaimType);
                    result = await _userManager.RemoveClaimsAsync(user, toremove);
                    if (!result.Succeeded) return CommandResult.Failure(result.Errors.Select(x => x.Description).ToArray());
                }

                result = await _userManager.AddClaimAsync(user, new Claim(message.ClaimType, message.ClaimValue));
                if (!result.Succeeded) return CommandResult.Failure(result.Errors.Select(x => x.Description).ToArray());

                _logger.Debug("Claim update for {user} with {@data}", message.Email, message);

                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error updating claim for {user}: @exception", message.Email, ex);
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}
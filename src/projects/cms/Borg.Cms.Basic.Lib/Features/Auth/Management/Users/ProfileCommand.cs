using Borg.Infra.DAL;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Auth.Management.Users
{
    public class ProfileCommand : CommandBase<CommandResult>
    {
        public ProfileCommand()
        {
        }

        public ProfileCommand(string email, string firstName, string lastName)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
    
        }

        [Required]
        public string Email { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }


    }

    public class ProfileCommandHandler : AsyncRequestHandler<ProfileCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly UserManager<CmsUser> _userManager;

        public ProfileCommandHandler(ILoggerFactory loggerFactory, UserManager<CmsUser> userManager)
        {
            _logger = loggerFactory.CreateLogger(GetType());

            _userManager = userManager;
        }

        protected override async Task<CommandResult> HandleCore(ProfileCommand message)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(message.Email);
                if (user == null)
                {
                    _logger.Debug("Cannot update profile, no user : {user}", message.Email);
                    return CommandResult.Failure($"Cannot update profile, no user : {message.Email}");
                }

                var claims = await _userManager.GetClaimsAsync(user);

                IdentityResult result;

                if (claims.Any(x => x.Type == ClaimTypes.Surname || x.Type == ClaimTypes.GivenName))
                {
                    var toremove = claims.Where(x =>
                        x.Type == ClaimTypes.Surname || x.Type == ClaimTypes.GivenName);
                    result = await _userManager.RemoveClaimsAsync(user, toremove);
                    if (!result.Succeeded) return CommandResult.Failure(result.Errors.Select(x => x.Description).ToArray());
                }

                result = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, message.FirstName));
                if (!result.Succeeded) return CommandResult.Failure(result.Errors.Select(x => x.Description).ToArray());

                result = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, message.LastName));
                if (!result.Succeeded) return CommandResult.Failure(result.Errors.Select(x => x.Description).ToArray());

                _logger.Debug("Profile update for {user} with {@data}", message.Email, message);

                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error updating profile for {user}: @exception", message.Email, ex);
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}
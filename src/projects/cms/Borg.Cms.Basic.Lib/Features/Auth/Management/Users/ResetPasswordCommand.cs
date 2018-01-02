using Borg.Infra.DAL;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Auth.Management.Users
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }
    }

    public class ResetPasswordCommand : CommandBase<CommandResult>
    {
        public ResetPasswordCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }
    }

    public class ResetPasswordCommandHandler : AsyncRequestHandler<ResetPasswordCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly UserManager<CmsUser> _userManager;

        public ResetPasswordCommandHandler(ILoggerFactory loggerFactory, UserManager<CmsUser> userManager)
        {
            _logger = loggerFactory.CreateLogger(GetType());

            _userManager = userManager;
        }

        protected override async Task<CommandResult> HandleCore(ResetPasswordCommand message)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(message.Email);
                if (user == null)
                {
                    _logger.Debug("Can not reset password, no user : {user}", message.Email);
                    return CommandResult.Failure($"Can not toggle lock, no user : {message.Email}");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, message.Password);

                if (result.Succeeded)
                {
                    return CommandResult.Success();
                }

                return CommandResult.Failure(result.Errors.Select(x => x.Description).ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error registering new user: @exception", ex);
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}
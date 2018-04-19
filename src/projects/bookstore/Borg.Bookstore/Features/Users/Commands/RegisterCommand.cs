using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.Auth.Register;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using Borg.Platform.Identity.Data;
using Borg.Platform.Identity.Data.Entities;
using Borg.Platform.MediatR;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace Borg.Bookstore.Features.Users.Commands
{
    public class RegisterCommand : CommandBase<CommandResult>
    {
        public RegisterCommand(RegisterViewModel model)
        {
            Model = model;
        }

        public RegisterViewModel Model { get; }
    }
    public class RegisterCommandHandler : AsyncRequestHandler<RegisterCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly UserManager<GenericUser> _userManager;
        private readonly IUnitOfWork<AuthDbContext> _uow;

        public RegisterCommandHandler(ILoggerFactory loggerFactory, UserManager<GenericUser> userManager, IUnitOfWork<AuthDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(typeof(RegisterCommandHandler));
            _userManager = userManager;
            _uow = uow;
        }

        protected override async Task<CommandResult> HandleCore(RegisterCommand message)
        {
            try
            {
                var repo = _uow.ReadWriteRepo<RegistrationRequest>();

                var requests = await repo.Find(x => x.Email == message.Model.Email, null);
                if (!requests.Any()) return CommandResult.Failure($"Could not find verification code for {message.Model.Email}");
                var hit = requests.OrderByDescending(x => x.SubmitedOn).First();
                if (hit.Code != message.Model.VerificationCode) return CommandResult.Failure($"Wrong verification code for {message.Model.Email}");

                var model = message.Model;
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null) return CommandResult.Failure($"No user for email {model.Email}");

                var results = new List<IdentityResult>
                {
                    await _userManager.SetLockoutEnabledAsync(user, false)
                };
                if (results.Any(x => !x.Succeeded))
                {
                    await _userManager.DeleteAsync(user);
                    _logger.LogDebug("deleting {@user}", model.Email);
                    return CommandResult.Failure(results.SelectMany(x => x.Errors).Select(x => x.Description).ToArray());
                }

                var deleteTasks = requests.Select(x => repo.Delete(r => r.Email == message.Model.Email));

                await Task.WhenAll(deleteTasks).AnyContext();
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error registering new user: @exception", ex);
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}
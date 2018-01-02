using Borg.Cms.Basic.Lib.Features.Auth.Data;
using Borg.Cms.Basic.Lib.Features.Auth.Register;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Auth.Management.Users
{
    public class DeleteUserCommand : CommandBase<CommandResult>
    {
        public string Email { get; set; }
    }

    public class DeleteUserCommandHandler : AsyncRequestHandler<DeleteUserCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly UserManager<CmsUser> _manager;
        private readonly IUnitOfWork<AuthDbContext> _requestsRepo;

        public DeleteUserCommandHandler(ILoggerFactory loggerFactory, UserManager<CmsUser> manager, IUnitOfWork<AuthDbContext> requestsRepo)
        {
            _manager = manager;
            _requestsRepo = requestsRepo;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<CommandResult> Handle(DeleteUserCommand message)
        {
            try
            {
                var repo = _requestsRepo.ReadWriteRepo<RegistrationRequest>();
                var pendingRequests = await repo.Find(x => x.Email == message.Email, null);
                var registrationRequests = pendingRequests.Records as RegistrationRequest[] ?? pendingRequests.ToArray();
                if (registrationRequests.Any())
                {
                    await repo.Delete(x => x.Email == message.Email);
                    await _requestsRepo.Save();
                }
                var user = await _manager.FindByEmailAsync(message.Email);
                if (user == null) return CommandResult.Failure($"Cannot delete user {message.Email} because it was not found.");
                _logger.Info("Deleting {@user}", user);
                var result = await _manager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return CommandResult.Failure($"Cannot delete user {message.Email} because {string.Join(", ", result.Errors.Select(x => x.Description))}");
                }
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting {user}: {@exception}", message.Email, ex);
                return CommandResult.Failure(ex.ToString());
            }
        }

        protected override async Task<CommandResult> HandleCore(DeleteUserCommand request)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class DeleteUserCommand : CommandBase<CommandResult>
    {
        public string Email { get; set; }
    }

    public class DeleteUserCommandHandler : AsyncRequestHandler<DeleteUserCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly UserManager<GenericUser> _manager;
        private readonly IUnitOfWork<AuthDbContext> _requestsRepo;

        public DeleteUserCommandHandler(ILoggerFactory loggerFactory, UserManager<GenericUser> manager, IUnitOfWork<AuthDbContext> requestsRepo)
        {
            _manager = manager;
            _requestsRepo = requestsRepo;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(DeleteUserCommand request)
        {
            try
            {
                var repo = _requestsRepo.ReadWriteRepo<RegistrationRequest>();
                var pendingRequests = await repo.Find(x => x.Email == request.Email, null);
                var registrationRequests = pendingRequests.Records as RegistrationRequest[] ?? pendingRequests.ToArray();
                if (registrationRequests.Any())
                {
                    await repo.Delete(x => x.Email == request.Email);
                    await _requestsRepo.Save();
                }
                var user = await _manager.FindByEmailAsync(request.Email);
                if (user == null) return CommandResult.Failure($"Cannot delete user {request.Email} because it was not found.");
                _logger.Info("Deleting {@user}", user);
                var result = await _manager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return CommandResult.Failure($"Cannot delete user {request.Email} because {string.Join(", ", result.Errors.Select(x => x.Description))}");
                }
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting {user}: {@exception}", request.Email, ex);
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}
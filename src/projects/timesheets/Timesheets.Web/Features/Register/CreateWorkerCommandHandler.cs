using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Timesheets.Web.Auth;
using Timesheets.Web.Domain;
using Timesheets.Web.Domain.Infrastructure;
using Borg.Platform.EF.Contracts;
using Borg.Infra.DAL;

namespace Timesheets.Web.Features.Register
{
    public class CreateWorkerCommandHandler : IAsyncRequestHandler<CreateWorkerCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork< TimesheetsDbContext> _uow;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateWorkerCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<TimesheetsDbContext> uow, UserManager<ApplicationUser> userManager)
        {
            _logger = loggerFactory.CreateLogger(typeof(CreateWorkerCommandHandler));
            _uow = uow;
            _userManager = userManager;
        }

        public async Task<CommandResult> Handle(CreateWorkerCommand message)
        {
            try
            {
                var model = message.Model;

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded) return CommandResult.Failure(result.Errors.Select(x=>x.Description).ToArray());
                result = await _userManager.AddToRoleAsync(user, Roles.Employee.ToString());
                if (!result.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    _logger.LogDebug("deleting @user",model.Email);
                    return CommandResult.Failure(result.Errors.Select(x => x.Description).ToArray());
                }

                var teamRepo = _uow.ReadWriteRepo<Team>();
                var workerRepo = _uow.ReadWriteRepo<Worker>();

                //var team = await _db.Teams.FirstOrDefaultAsync(x => x.Id.Equals(model.Team.ToString(), StringComparison.OrdinalIgnoreCase));
                var team = await teamRepo.Get(x => x.Id.Equals(model.Team.ToString(), StringComparison.OrdinalIgnoreCase));
                if (team == null)
                {
                    await _userManager.DeleteAsync(user);
                    _logger.LogDebug("deleting @user", model.Email);
                    return CommandResult.Failure($"Team {model.Team} is invalid.");
                }
                //var worker = await _db.Workers.FirstOrDefaultAsync(x => x.Id.Equals(model.Email, StringComparison.OrdinalIgnoreCase));
                var worker = await  workerRepo.Get(x => x.Id.Equals(model.Email, StringComparison.OrdinalIgnoreCase));
                if (worker != null)
                {
                    await _userManager.DeleteAsync(user);
                    _logger.LogDebug("deleting @user", model.Email);
                    return CommandResult.Failure($"Employee with email {model.Email} exists.");
                }
                worker = new Worker(model.Email, model.Team)
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                //await _db.Workers.AddAsync(worker);
                //await _db.SaveChangesAsync();

                await workerRepo.Create(worker);
                await _uow.Save();

                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1,ex,"Error registering new user: @exception", ex);
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Timesheets.Web.Domain;
using Web.Features.Workers;

using Borg.Platform.EF.Contracts;
using Borg.Infra.DAL;

namespace Timesheets.Web.Features.Workers
{
    public class NameAndTeamCommand : IRequest<CommandResult>
    {
        public NameAndTeamCommand(WorkerNameAndTeamViewModel model)
        {
            Model = model;
        }

        public WorkerNameAndTeamViewModel Model { get; }

    }

    public class NameAndTeamCommandHandler : IAsyncRequestHandler<NameAndTeamCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<TimesheetsDbContext> _uow;


        public NameAndTeamCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<TimesheetsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(typeof(ToggleLockOutCommandHandler));

            _uow = uow;
        }

        public async Task<CommandResult> Handle(NameAndTeamCommand message)
        {
            try
            {
                var repo = _uow.ReadWriteRepo<Worker>();
                //var worker = await _db.Workers.FindAsync(message.Model.Email);
                var worker = await repo.Get(x => x.Id.Equals(message.Model.Email));


                if (worker == null)
                {
                    _logger.LogDebug(1, "Can not set name and team, no user : {user}", message.Model.Email);
                    return CommandResult.Failure($"Can not set name and team, no user : {message .Model.Email}");
                }

                worker.SetTeam( message.Model.Team);
                worker.FirstName = message.Model.FirstName;
                worker.LastName = message.Model.LaststName;
                await repo.Update(worker);
                await _uow.Save();
                return CommandResult.Success();


            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error setting name and team for user {user} - {@exception}",message.Model.Email, ex);
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}

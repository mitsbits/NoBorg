using Borg.Cms.Basic.Lib.Features.Navigation.Events;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.Infra.DTO;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Navigation.Commands
{
    public class NavigationItemRecordDeleteCommand : CommandBase<CommandResult>
    {
        public int RecordId { get; set; }
        public string Group { get; set; }
    }

    public class NavigationItemRecordDeleteCommandHandler : AsyncRequestHandler<NavigationItemRecordDeleteCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<BorgDbContext> _uow;
        private readonly IMediator _dispatcher;

        public NavigationItemRecordDeleteCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow, IMediator dispatcher)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _dispatcher = dispatcher;
        }

        protected override async Task<CommandResult> HandleCore(NavigationItemRecordDeleteCommand message)
        {
            try
            {
                //var repo = _uow.ReadWriteRepo<NavigationItemRecord>();
                //var set = await repo.Find(x => x.Group == message.Group, null);
                //var existing = set.FirstOrDefault(x => x.Id == message.RecordId);
                //if (existing == null) return CommandResult.Failure($"Navigation Item with Id {message.RecordId} is not present, delete fails");
                //var tree = set.Trees(existing.Id);
                //var idstodelete = tree.Flatten().Select(x => int.Parse(x.Key)).Union(new[] { existing.Id });
                //foreach (var i in idstodelete)
                //{
                //    await repo.Delete(x => x.Id == i);
                //}
                //await _uow.Save();
                //var tasks = idstodelete.Select(x =>
                //    _dispatcher.Publish(new NavigationItemRecordStateChanged(x, DmlOperation.Delete)));
                //await Task.WhenAll(tasks);
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error deleting menu {id} in set {set}.: {@exception}", message.RecordId, message.Group, ex);
                return CommandResult<NavigationItemRecord>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}
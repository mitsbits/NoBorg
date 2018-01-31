using Borg.Cms.Basic.Lib.Features.CMS.Events;
using Borg.Infra.DAL;
using Borg.Infra.DTO;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Commands
{
    public class NavigationItemStateDeleteCommand : CommandBase<CommandResult>
    {
        public int RecordId { get; set; }
        public string Group { get; set; }
    }

    public class NavigationItemStateDeleteCommandHandler : AsyncRequestHandler<NavigationItemStateDeleteCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;
        private readonly IMediator _dispatcher;

        public NavigationItemStateDeleteCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _dispatcher = dispatcher;
        }

        protected override async Task<CommandResult> HandleCore(NavigationItemStateDeleteCommand message)
        {
            try
            {
                var set = await _uow.Context.NavigationItemStates.Include(x => x.Taxonomy).ThenInclude(x => x.Component)
                    .Include(x => x.Taxonomy).ThenInclude(x => x.Article)
                    .Where(x => x.GroupCode.ToLower() == message.Group).ToListAsync();

                var existing = set.FirstOrDefault(x => x.Id == message.RecordId);
                if (existing == null) return CommandResult.Failure($"Navigation Item with Id {message.RecordId} is not present, delete fails");
                var tree = set.Trees(existing.Id);
                var idstodelete = tree.Flatten().Select(x => int.Parse(x.Key)).Union(new[] { existing.Id });
                foreach (var i in idstodelete)
                {
                    var hit = await _uow.ReadWriteRepo<ComponentState>().Get(x => x.Id == i);
                    hit.IsDeleted = true;
                }
                await _uow.Save();
                var tasks = idstodelete.Select(x =>
                    _dispatcher.Publish(new NavigationItemStateChanged(x, DmlOperation.Delete)));
                await Task.WhenAll(tasks);
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error deleting menu {id} in set {set}.: {@exception}", message.RecordId, message.Group, ex);
                return CommandResult<NavigationItemState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}
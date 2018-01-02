using Borg.Cms.Basic.Lib.Features.Navigation.Events;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.Infra.DTO;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Navigation.Commands
{
    public class NavigationItemRecordCreateOrUpdateCommand : CommandBase<CommandResult<NavigationItemRecord>>
    {
        public NavigationItemRecord Record { get; set; }
        public IDictionary<(int, int), Tiding> ParentOptions { get; set; }
        public IEnumerable<NavigationItemType> NavigaionTypeOptions => EnumUtil.GetValues<NavigationItemType>();
    }

    public class NavigationItemRecordCreateOrUpdateCommandHandler : AsyncRequestHandler<NavigationItemRecordCreateOrUpdateCommand, CommandResult<NavigationItemRecord>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<BorgDbContext> _uow;
        private readonly IMediator _dispatcher;

        public NavigationItemRecordCreateOrUpdateCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow, IMediator dispatcher)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _dispatcher = dispatcher;
        }

        protected override async Task<CommandResult<NavigationItemRecord>> HandleCore(NavigationItemRecordCreateOrUpdateCommand message)
        {
            try
            {
                var repo = _uow.ReadWriteRepo<NavigationItemRecord>();
                var set = await repo.Find(x => x.Group == message.Record.Group, null);
                var existing = set.FirstOrDefault(x => x.Id == message.Record.Id);

                if (existing == null)
                {
                    if (message.Record.ParentId != 0 && !set.Any(x => x.Id == message.Record.Id))
                        return CommandResult<NavigationItemRecord>.FailureWithEmptyPayload($"Could not resolve parent for {message.Record.Display} in set {message.Record.Group}.");
                    var entity = new NavigationItemRecord()
                    {
                        Group = message.Record.Group,
                        Path = message.Record.Path,
                        Display = message.Record.Display,
                        ParentId = message.Record.ParentId,
                        ItemType = message.Record.ItemType,
                        IsPublished = message.Record.IsPublished
                    };
                    if (message.Record.IsPublished) { entity.Publish(); } else { entity.Suspend(); }
                    await repo.Create(entity);
                    await _uow.Save();
                    _logger.Info("Created menu {@menu}", entity);
                    await _dispatcher.Publish(new NavigationItemRecordStateChanged(entity.Id, DmlOperation.Create));
                    return CommandResult<NavigationItemRecord>.Success(entity);
                }
                else
                {
                    _uow.Context.Entry(existing).State = EntityState.Detached;
                    _uow.Context.Attach(message.Record);
                    _uow.Context.Entry(message.Record).State = EntityState.Modified;
                    await _uow.Save();
                    _logger.Info("Update menu from {@old} to {@new}", existing, message.Record);
                    await _dispatcher.Publish(new NavigationItemRecordStateChanged(message.Record.Id, DmlOperation.Create));
                    return CommandResult<NavigationItemRecord>.Success(existing);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error updating or creating menu {display} in set {group}.: {@exception}", message.Record.Display, message.Record.Group, ex);
                return CommandResult<NavigationItemRecord>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}
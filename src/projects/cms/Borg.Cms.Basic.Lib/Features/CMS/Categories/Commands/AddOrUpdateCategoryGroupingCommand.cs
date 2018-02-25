using Borg.Cms.Basic.Lib.Features.CMS.Categories.Events;
using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Categories.Commands
{
    public class AddOrUpdateCategoryGroupingCommand : CommandBase<CommandResult<CategoryGroupingState>>
    {
        public int RecordId { get; set; }
        public string FriendlyName { get; set; }
    }

    public class AddOrUpdateCategoryGroupingCommandHandler : AsyncRequestHandler<AddOrUpdateCategoryGroupingCommand, CommandResult<CategoryGroupingState>>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        public AddOrUpdateCategoryGroupingCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult<CategoryGroupingState>> HandleCore(AddOrUpdateCategoryGroupingCommand message)
        {
            try
            {
                var isTransient = message.RecordId == default(int);
                CategoryGroupingStateChangedEvent @event = null;
                CategoryGroupingState record;
                if (isTransient)
                {
                    var component = new ComponentState();
                    await _uow.ReadWriteRepo<ComponentState>().Create(component);
                    record = new CategoryGroupingState() { Component = component, FriendlyName = message.FriendlyName };
                    await _uow.ReadWriteRepo<CategoryGroupingState>().Create(record);
                    await _uow.Save();
                    _logger.Info("Created Category Groupimg {@record}", record);
                    @event = new CategoryGroupingStateChangedEvent(record.Id, CRUDOperation.Create);
                }
                else
                {
                    record = await _uow.ReadWriteRepo<CategoryGroupingState>().Get(x => x.Id == message.RecordId);
                    record.FriendlyName = message.FriendlyName;
                    await _uow.ReadWriteRepo<CategoryGroupingState>().Update(record);
                    await _uow.Save();
                    @event = new CategoryGroupingStateChangedEvent(record.Id, CRUDOperation.Update);
                }
                if (@event != null) _dispatcher.Publish(@event);
                return CommandResult<CategoryGroupingState>.Success(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating/updating html snippet from {@message} - {exception}", message, ex.ToString());
                return CommandResult<CategoryGroupingState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}
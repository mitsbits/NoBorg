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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.Content.Commands;
using Borg.Infra.Services.Slugs;

namespace Borg.Cms.Basic.Lib.Features.Navigation.Commands
{
    public class NavigationItemRecordCreateOrUpdateCommand : CommandBase<CommandResult<NavigationItemRecord>>
    {
        [Required]
        public int RecordId { get; set; } = 0;


        [DefaultValue(0)]
        [DisplayName("Parent")]
        [Required]
        public int ParentId { get; set; }

        [DisplayName("Display")]
        [Required]
        public string Display { get; set; }

        [DefaultValue("BSE")]
        [DisplayName("Group")]
        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Group { get; set; }

        [DefaultValue("/")]
        [DisplayName("Path")]
        public string Path { get; set; }

        [DisplayName("Item Type")]
        [Required]
        public NavigationItemType ItemType { get; set; } = NavigationItemType.Label;

        [DefaultValue(true)]
        [DisplayName("Active")]
        [Required]
        public bool IsPublished { get; set; }

        [DefaultValue(0)]
        [DisplayName("Weight")]
        public double Weight { get; set; }

        public IDictionary<(int, int), Tiding> ParentOptions { get; set; }
        public IEnumerable<NavigationItemType> NavigaionTypeOptions => EnumUtil.GetValues<NavigationItemType>();
    }

    public class NavigationItemRecordCreateOrUpdateCommandHandler : AsyncRequestHandler<NavigationItemRecordCreateOrUpdateCommand, CommandResult<NavigationItemRecord>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<BorgDbContext> _uow;
        private readonly IMediator _dispatcher;
        private readonly ISlugifierService _slugifier;

        public NavigationItemRecordCreateOrUpdateCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow, IMediator dispatcher, ISlugifierService slugifier)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _dispatcher = dispatcher;
            _slugifier = slugifier;
        }

        protected override async Task<CommandResult<NavigationItemRecord>> HandleCore(NavigationItemRecordCreateOrUpdateCommand message)
        {
            try
            {
                var repo = _uow.ReadWriteRepo<NavigationItemRecord>();
                var set = await repo.Find(x => x.Group == message.Group, null);
                var existing = set.FirstOrDefault(x => x.Id == message.RecordId);

                var entity = new NavigationItemRecord()
                {
                    Group = message.Group,
                    Path = message.Path,
                    Display = message.Display,
                    ParentId = message.ParentId,
                    ItemType = message.ItemType,
                    IsPublished = message.IsPublished,
                    Weight = message.Weight
                };

                if (existing == null)
                {
                    if (message.ParentId > 0 && !set.Any(x => x.Id == message.ParentId))
                        return CommandResult<NavigationItemRecord>.FailureWithEmptyPayload($"Could not resolve parent for {message.Display} in set {message.Group}.");

                    await repo.Create(entity);
                    var contentCommand = new ContentItemCreateOrUpdateCommand(entity.Display,
                        _slugifier.Slugify(entity.Display), "", "", DateTimeOffset.UtcNow, "system",
                        default(DateTimeOffset?));
                    var contentResult = await _dispatcher.Send(contentCommand);
                    if (contentResult.Succeded)
                    {
                        entity.ContentItemRecordId = contentResult.Payload.Id;
                    }
                    await _uow.Save();
                    _logger.Info("Created menu {@menu}", entity);
                    await _dispatcher.Publish(new NavigationItemRecordStateChanged(entity.Id, DmlOperation.Create));
                    return CommandResult<NavigationItemRecord>.Success(entity);
                }
                else
                {
                    var existingContentId = existing.ContentItemRecordId;
                    if (!existingContentId.HasValue)
                    {
                        var contentCommand = new ContentItemCreateOrUpdateCommand(entity.Display,
                            _slugifier.Slugify(entity.Display), "", "", DateTimeOffset.UtcNow, "system",
                            default(DateTimeOffset?));
                        var contentResult = await _dispatcher.Send(contentCommand);
                        if (contentResult.Succeded)
                        {
                            existingContentId = contentResult.Payload.Id;
                        }
                    }
                    _uow.Context.Entry(existing).State = EntityState.Detached;
                    entity.Id = message.RecordId;
                    entity.ContentItemRecordId = existingContentId;
                    _uow.Context.Attach(entity);
                    _uow.Context.Entry(entity).State = EntityState.Modified;
                    await _uow.Save();
                    _logger.Info("Update menu from {@old} to {@new}", existing, message);
                    await _dispatcher.Publish(new NavigationItemRecordStateChanged(message.RecordId, DmlOperation.Create));
                    return CommandResult<NavigationItemRecord>.Success(existing);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error updating or creating menu {display} in set {group}.: {@exception}", message.Display, message.Group, ex);
                return CommandResult<NavigationItemRecord>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}
using Borg.Cms.Basic.Lib.Features.CMS.Events;
using Borg.CMS;
using Borg.Infra.DAL;
using Borg.Infra.DTO;
using Borg.Infra.Services.Slugs;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
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

namespace Borg.Cms.Basic.Lib.Features.CMS.Commands
{
    public class NavigationItemStateCreateOrUpdateCommand : CommandBase<CommandResult<NavigationItemState>>
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

        [DefaultValue(true)]
        [DisplayName("IsDeleted")]
        [Required]
        public bool IsDeleted { get; set; }

        [DefaultValue(0)]
        [DisplayName("Weight")]
        public double Weight { get; set; }

        public IDictionary<(int, int), Tiding> ParentOptions { get; set; }
        public IEnumerable<NavigationItemType> NavigaionTypeOptions => EnumUtil.GetValues<NavigationItemType>();
    }

    public class NavigationItemStateCreateOrUpdateCommandHandler : AsyncRequestHandler<NavigationItemStateCreateOrUpdateCommand, CommandResult<NavigationItemState>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;
        private readonly IMediator _dispatcher;
        private readonly ISlugifierService _slugifier;

        public NavigationItemStateCreateOrUpdateCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher, ISlugifierService slugifier)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _dispatcher = dispatcher;
            _slugifier = slugifier;
        }

        protected override async Task<CommandResult<NavigationItemState>> HandleCore(NavigationItemStateCreateOrUpdateCommand message)
        {
            try
            {
                var set = await _uow.Context.NavigationItemStates.Include(x => x.Taxonomy).ThenInclude(x => x.Component)
                    .Include(x => x.Taxonomy).ThenInclude(x => x.Article)
                    .Where(x => x.GroupCode.ToLower() == message.Group).ToListAsync();

                var existing = set.FirstOrDefault(x => x.Id == message.RecordId);
                if (existing == null)
                {
                    var comp = new ComponentState() { IsPublished = message.IsPublished };
                    await _uow.ReadWriteRepo<ComponentState>().Create(comp);
                    var art = new ArticleState() { Component = comp, Title = message.Display };
                    await _uow.ReadWriteRepo<ArticleState>().Create(art);
                    var tx = new TaxonomyState() { Component = comp, Article = art, ParentId = message.ParentId, Weight = message.Weight };
                    await _uow.ReadWriteRepo<TaxonomyState>().Create(tx);

                    var entity = new NavigationItemState
                    {
                        GroupCode = message.Group,
                        NavigationItemType = message.ItemType,
                        Path = message.Path,
                        Taxonomy = tx,
                        Id = comp.Id,
                        Display = message.Display
                    };

                    if (message.ParentId > 0 && !set.Any(x => x.Id == message.ParentId))
                        return CommandResult<NavigationItemState>.FailureWithEmptyPayload($"Could not resolve parent for {message.Display} in set {message.Group}.");

                    await _uow.ReadWriteRepo<NavigationItemState>().Create(entity);

                    await _uow.Save();
                    _logger.Info("Created menu {@menu}", entity);
                    await _dispatcher.Publish(new NavigationItemStateChanged(entity.Id, DmlOperation.Create));
                    return CommandResult<NavigationItemState>.Success(entity);
                }
                else
                {
                    existing.GroupCode = message.Group;
                    existing.Taxonomy.ParentId = message.ParentId;
                    existing.Taxonomy.Weight = message.Weight;
                    existing.Taxonomy.Component.IsPublished = message.IsPublished;
                    existing.Path = message.Path;
                    existing.NavigationItemType = message.ItemType;
                    existing.Display = message.Group;
                    //await _uow.ReadWriteRepo<NavigationItemState>().Update(entity);
                    await _uow.Save();
                    _logger.Info("Update menu from {@old} to {@new}", existing, message);
                    await _dispatcher.Publish(new NavigationItemStateChanged(message.RecordId, DmlOperation.Create));
                    return CommandResult<NavigationItemState>.Success(existing);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error updating or creating menu {display} in set {group}.: {@exception}", message.Display, message.Group, ex);
                return CommandResult<NavigationItemState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}
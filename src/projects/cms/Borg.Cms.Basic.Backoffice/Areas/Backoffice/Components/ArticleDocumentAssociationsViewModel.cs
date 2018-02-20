using Borg.Cms.Basic.Lib.Features.CMS.Commands;
using Borg.Infra.DAL;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Components
{
    public class ArticleDocumentAssociationsViewComponent : ViewComponentModule<Tidings>
    {
        private readonly IUnitOfWork<CmsDbContext> _uow;
        private readonly ILogger _logger;

        public ArticleDocumentAssociationsViewComponent(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        public async Task<IViewComponentResult> InvokeAsync(Tidings tidings)
        {
            if (!tidings.ContainsKey(Tidings.DefinedKeys.Id))
            {
                _logger.Error(new ArgumentNullException("component id"), "{component} failed to render", nameof(ArticlePageMetadataViewComponent));
                return null;
            }
            if (!int.TryParse(tidings[Tidings.DefinedKeys.Id], out int id))
            {
                _logger.Error(new ArgumentNullException("component id"), "{component} failed to render", nameof(ArticlePageMetadataViewComponent));
                return null;
            }
            var model = new ArticleDocumentAssociationsViewModel
            {
                Command = new AppendDocumentToComponentCommand() { ComponentId = id }
            };

            var collection = await _uow.QueryRepo<ComponentDocumentAssociationState>().Find(x => x.ComponentId == id,
                SortBuilder.Get<ComponentDocumentAssociationState>(x => x.ComponentId, false).Build());

            model.Associations = collection;

            if (tidings.ContainsKey(Tidings.DefinedKeys.View) && !string.IsNullOrWhiteSpace(tidings[Tidings.DefinedKeys.View])) return View(tidings[Tidings.DefinedKeys.View], model);
            return View(model);
        }
    }

    public class ArticleDocumentAssociationsViewModel
    {
        public AppendDocumentToComponentCommand Command { get; set; }
        public IEnumerable<ComponentDocumentAssociationState> Associations { get; set; }
    }
}
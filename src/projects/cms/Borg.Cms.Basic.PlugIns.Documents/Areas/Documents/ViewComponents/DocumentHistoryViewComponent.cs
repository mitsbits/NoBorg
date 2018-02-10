using Borg.Cms.Basic.PlugIns.Documents.Data;
using Borg.Cms.Basic.PlugIns.Documents.ViewModels;
using Borg.Infra.DAL;
using Borg.Infra.DTO;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.MVC.BuildingBlocks;
using Borg.Platform.EF.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.ViewComponents
{
    public class DocumentHistoryViewComponent : ViewComponentModule<Tidings>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<DocumentsDbContext> _uow;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;

        public DocumentHistoryViewComponent(IUnitOfWork<DocumentsDbContext> uow, IAssetStore<AssetInfoDefinition<int>, int> assetStore, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _assetStore = assetStore;
        }

        public async Task<IViewComponentResult> InvokeAsync(Tidings tidings)
        {
            if (!tidings.ContainsKey(Tidings.DefinedKeys.Id))
            {
                _logger.Error(new ArgumentNullException("document id"), "{component} failed to render", nameof(DocumentHistoryViewComponent));
                return null;
            }
            if (!int.TryParse(tidings[Tidings.DefinedKeys.Id], out int id))
            {
                _logger.Error(new ArgumentNullException("document id"), "{component} failed to render", nameof(DocumentHistoryViewComponent));
                return null;
            }

            var versions = new List<IVersionInfo>();
            var checkouts = new List<DocumentCheckOutState>();

            var task1 = Task.Run(async () =>
            {
                versions.AddRange(await _assetStore.AssetVersions(id));
            });

            var task2 = Task.Run(async () =>
            {
                checkouts.AddRange(await _uow.QueryRepo<DocumentCheckOutState>().Find(x => x.DocumentId == id, SortBuilder.Get<DocumentCheckOutState>().Add(d => d.CheckOutVersion).Build()));
            });

            await Task.WhenAll(task1, task2);
            var model = new DocumentHistoryViewModel(versions, checkouts);
            if (tidings.ContainsKey(Tidings.DefinedKeys.View) && !string.IsNullOrWhiteSpace(tidings[Tidings.DefinedKeys.View])) return View(tidings[Tidings.DefinedKeys.View], model);
            return View(model);
        }
    }
}
using Borg.Cms.Basic.PlugIns.Documents.Data;
using Borg.Cms.Basic.PlugIns.Documents.ViewModels;
using Borg.Infra.DAL;
using Borg.Infra.DTO;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.MVC.BuildingBlocks;
using Borg.Platform.EF.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.ViewComponents
{
    public class LatestUserDocumentsViewComponent : ViewComponentModule<Tidings>
    {

        private const string _countKey = "count";

        private readonly IUnitOfWork<DocumentsDbContext> _uow;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;

        public LatestUserDocumentsViewComponent(IUnitOfWork<DocumentsDbContext> uow, IAssetStore<AssetInfoDefinition<int>, int> assetStore)
        {
            _uow = uow;
            _assetStore = assetStore;
        }

        public async Task<IViewComponentResult> InvokeAsync(Tidings tidings)
        {
            var count = tidings.ContainsKey(_countKey) ? int.Parse(tidings[_countKey]) : 6;
            var q = from o in _uow.Context.DocumentOwnerStates.Include(x => x.Document).ThenInclude(x => x.Owners)
                    .AsNoTracking()
                    where o.Owner == User.Identity.Name
                    orderby o.AssociatedOn descending
                    select o;

            var docs = await q.Take(count).ToListAsync();

            var assets = await _assetStore.Projections(docs.Select(x => x.DocumentId));

            var bucket = new List<DocumentForOwnerViewModel>();

            foreach (var documentOwnerState in docs)
            {
                bucket.Add(new DocumentForOwnerViewModel()
                {
                    Owner = documentOwnerState,
                    Document = documentOwnerState.Document,
                    Asset = assets.FirstOrDefault(x => x.Id == documentOwnerState.DocumentId)
                });
            }
            if (tidings.ContainsKey(Tidings.DefinedKeys.View) && !string.IsNullOrWhiteSpace(tidings[Tidings.DefinedKeys.View])) return View(tidings[Tidings.DefinedKeys.View], bucket);
            return View(bucket);
        }
    }
}
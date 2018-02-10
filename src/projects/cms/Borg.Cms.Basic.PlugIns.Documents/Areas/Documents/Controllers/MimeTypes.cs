using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.Controllers
{
    public class MimeTypesController : DocumentsController
    {
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;

        public MimeTypesController(ILoggerFactory loggerFactory, IAssetStore<AssetInfoDefinition<int>, int> assetStore) : base(loggerFactory)
        {
            _assetStore = assetStore;
        }

        [HttpGet]
        public async Task<IActionResult> Home()
        {
            return View(await _assetStore.MimeTypes());
        }
    }
}
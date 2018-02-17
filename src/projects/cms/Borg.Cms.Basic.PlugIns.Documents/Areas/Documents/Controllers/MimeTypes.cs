using Borg.Cms.Basic.PlugIns.Documents.Queries;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.Controllers
{
    public class MimeTypesController : DocumentsController
    {
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;
        private readonly IMediator _dispatcher;

        public MimeTypesController(ILoggerFactory loggerFactory, IAssetStore<AssetInfoDefinition<int>, int> assetStore, IMediator dispatcher) : base(loggerFactory)
        {
            _assetStore = assetStore;
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Home()
        {
            return View(await _assetStore.MimeTypes());
        }

        [HttpGet]
        public async Task<IActionResult> Groupings()
        {
            SetPageTitle("Mime Type Groupings");
            var result = await _dispatcher.Send(new MimeTypeGroupingRowsRequest());
            return View(result.Payload);
        }
    }
}
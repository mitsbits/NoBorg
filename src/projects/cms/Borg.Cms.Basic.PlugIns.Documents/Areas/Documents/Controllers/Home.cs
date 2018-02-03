using Borg.Infra.Storage.Assets.Contracts;
using Borg.MVC.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.Controllers
{
    [ControllerTheme("Backoffice")]
    public class HomeController : DocumentsController
    {
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;

        public HomeController(ILoggerFactory loggerFactory, IAssetStore<AssetInfoDefinition<int>, int> assetStore) : base(loggerFactory)
        {
            _assetStore = assetStore;
        }

        public IActionResult Home()
        {
            SetPageTitle("docs home");
            return View();
        }

        public async Task<IActionResult> Item(int id)
        {
            var hits = await _assetStore.Projections(new[] { id });
            if (!hits.Any()) return NotFound($"no document for id {id}");
            var doc = hits.First();
            SetPageTitle(doc.Name, doc.CurrentFile.FileSpec.MimeType);
            return View(doc);
        }
    }
}
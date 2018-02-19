using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Conventions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Presentation.Controllers
{
    //[TypeFilter(typeof(DeviceLayoutFilter), Arguments = new object[] { "Templates/_BlogLayout" })]
    [ControllerTheme("Bootstrap3")]
    public class HomeController : Controller
    {
        private readonly IDeviceStructureProvider _deviceProvider;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;

        public HomeController(IDeviceStructureProvider deviceProvider, IAssetStore<AssetInfoDefinition<int>, int> assetStore)
        {
            _deviceProvider = deviceProvider;
            _assetStore = assetStore;
        }

        public async Task<IActionResult> Home([FromServices] IPageOrchestrator<IPageContent, IDevice> orchestrator)
        {
            return View();
        }
    }
}
using Borg.Infra.Storage.Assets.Contracts;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Controllers
{
    //[TypeFilter(typeof(DeviceLayoutFilter), Arguments = new object[] { "Templates/_BlogLayout" })]
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
            orchestrator.TryContextualize(this);
            var model = await _deviceProvider.PageLayout(2);

            orchestrator.Device.Layout = model.Layout;
            orchestrator.Device.RenderScheme = model.RenderScheme;
            orchestrator.Device.SectionsClear();
            foreach (var modelSection in model.Sections)
            {
                orchestrator.Device.SectionAdd(modelSection);
            }
            return View();
        }

        [HttpGet]
        public IActionResult FileIt()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FileIt(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream);

                        await _assetStore.Create(formFile.Name, stream.ToArray(), formFile.FileName);
                    }
                }
            }
            return Ok(new { count = files.Count, size, filePath });
        }
    }
}
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Controllers
{
    //[TypeFilter(typeof(DeviceLayoutFilter), Arguments = new object[] { "Templates/_BlogLayout" })]
    public class HomeController : Controller
    {
        private readonly IDeviceStructureProvider _deviceProvider;

        public HomeController(IDeviceStructureProvider deviceProvider)
        {
            _deviceProvider = deviceProvider;
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
    }
}
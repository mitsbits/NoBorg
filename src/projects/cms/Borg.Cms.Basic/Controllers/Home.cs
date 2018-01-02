using System.Threading.Tasks;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Borg.Cms.Basic.Controllers
{
    //[TypeFilter(typeof(DeviceLayoutFilter), Arguments = new object[] { "_Layout" })]
    public class HomeController : Controller
    {
        private readonly IDeviceStructureProvider _deviceProvider;

        public HomeController(IDeviceStructureProvider deviceProvider)
        {
            _deviceProvider = deviceProvider;
        }

        public async Task< IActionResult> Home([FromServices] IPageOrchestrator<IPageContent, IDevice> orchestrator)
        {
            var model = await _deviceProvider.PageLayout(8);
            orchestrator.TryContextualize(this);
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
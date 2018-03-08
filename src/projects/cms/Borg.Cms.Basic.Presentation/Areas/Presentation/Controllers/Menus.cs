using Borg.Cms.Basic.Presentation.Queries;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.Conventions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Presentation.Areas.Presentation.Controllers
{
    [ControllerTheme("Bootstrap3")]
    public class MenusController : PresentationController
    {
        public MenusController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
        {
        }

        public async Task<IActionResult> SiteRoot()
        {
            var rootmenu = "home";
            var result = await Dispatcher.Send(new MenuRootPageContentRequest(rootmenu));
            if (!result.Succeded) return BadRequest($"no menu for path {rootmenu} was found");
            PageContent(result.Payload.content);
            var id = result.Payload.componentId;

            var structureresult = await Dispatcher.Send(new ComponentDeviceRequest(id));
            if (!structureresult.Succeded) return BadRequest($"no structure for path {rootmenu} was found");
            
            var device = this.GetDevice<Device>();
            device.Layout = structureresult.Payload.Layout;
            device.RenderScheme = structureresult.Payload.RenderScheme;

            foreach (var payloadSection in structureresult.Payload.Sections)
            {
                device.SectionAdd(payloadSection);
            }
            PageDevice(device);

            return View("Root");
        }

        public async Task<IActionResult> Root(string rootmenu)
        {
            var result = await Dispatcher.Send(new MenuRootPageContentRequest(rootmenu));
            if (!result.Succeded) return BadRequest($"no menu for path {rootmenu} was found");
            var id = result.Payload.componentId;

            var structureresult = await Dispatcher.Send(new ComponentDeviceRequest(id));
            if (!structureresult.Succeded) return BadRequest($"no structure for path {rootmenu} was found");
            PageContent(result.Payload.content);
            var device = this.GetDevice<Device>();
            device.Layout = structureresult.Payload.Layout;
            device.RenderScheme = structureresult.Payload.RenderScheme;

            foreach (var payloadSection in structureresult.Payload.Sections)
            {
                device.SectionAdd(payloadSection);
            }
            PageDevice(device);

            return View();
        }

        public async Task<IActionResult> Leaf(string parentmenu, string childmenu)
        {
            var result = await Dispatcher.Send(new MenuLeafPageContentRequest(parentmenu, childmenu));
            if (!result.Succeded) return BadRequest($"no menu for path {parentmenu}/{childmenu} was found");
            var id = result.Payload.componentId;

            var structureresult = await Dispatcher.Send(new ComponentDeviceRequest(id));
            if (!structureresult.Succeded) return BadRequest($"no structure for path {parentmenu}/{childmenu} was found");
            PageContent(result.Payload.content);
            var device = this.GetDevice<Device>();
            device.Layout = structureresult.Payload.Layout;
            device.RenderScheme = structureresult.Payload.RenderScheme;

            foreach (var payloadSection in structureresult.Payload.Sections)
            {
                device.SectionAdd(payloadSection);
            }
            PageDevice(device);

            return View();
        }
    }
}
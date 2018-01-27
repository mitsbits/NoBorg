using Borg.Cms.Basic.Lib.Features.Device.Commands;
using Borg.Cms.Basic.Lib.Features.Device.Queries;
using Borg.Cms.Basic.Lib.Features.Device.ViewModels;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Services.Breadcrumbs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    [Route("[area]/Devices")]
    public class DevicesController : BackofficeController
    {
        private readonly IModuleDescriptorProvider _modules;
        private readonly IDeviceLayoutFileProvider _deviceLayoutFiles;

        public DevicesController(ILoggerFactory loggerFactory, IMediator dispatcher, IModuleDescriptorProvider modules, IDeviceLayoutFileProvider deviceLayoutFiles) : base(loggerFactory, dispatcher)
        {
            _modules = modules;
            _deviceLayoutFiles = deviceLayoutFiles;
        }

        [HttpGet("")]
        public async Task<IActionResult> Home()
        {
            ViewBag.LayoutFiles = await _deviceLayoutFiles.LayoutFiles();
            var model = await Dispatcher.Send(new DevicesRequest());

            SetPageTitle("Devices", $"{model.Payload.Count()} templates");
            var device = PageDevice<Device>();
            device.Breadcrumbs.Add(new BreadcrumbLink("Devices", Url.Action("Home", "Devices")));
            PageDevice(device);
            return View(model.Payload);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Home(int id)
        {
            ViewBag.LayoutFiles = await _deviceLayoutFiles.LayoutFiles();

            var model = await Dispatcher.Send(new DeviceRequest(id));
            var layouts = await _deviceLayoutFiles.LayoutFiles();
            var selectedLayout = layouts.FirstOrDefault(x => x.MatchesPath(model.Payload.Layout));
            ViewBag.AvailableSectionIdentifiers = selectedLayout.SectionIdentifiers;
            SetPageTitle($"Device: {model.Payload.FriendlyName}", $"{model.Payload.Sections.Count} sections");
            var device = PageDevice<Device>();
            device.Breadcrumbs.Add(new BreadcrumbLink("Devices", Url.Action("Home", "Devices", new { id = "" })));
            device.Breadcrumbs.Add(new BreadcrumbLink(model.Payload.FriendlyName, Url.Action("Home", "Devices", new { id = model.Payload.Id })));
            PageDevice(device);
            return View("Device", model.Payload);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(DeviceCreateOrUpdateCommand model)
        {
            if (ModelState.IsValid)
            {
                var result = await Dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
                return RedirectToAction(nameof(Home), new { id = result.Payload.Id });
            }
            return RedirectToAction(nameof(Home), new { id = "" });
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(DeviceDeleteCommand model)
        {
            if (ModelState.IsValid)
            {
                var result = await Dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return RedirectToAction(nameof(Home));
        }

        [HttpGet("section/{id}")]
        public async Task<IActionResult> Section(int id)
        {
            var reposnse = await Dispatcher.Send(new SectionRequest(id));
            SetPageTitle($"Section: {reposnse.Payload.FriendlyName}", $"Device: {reposnse.Payload.Device.FriendlyName}");
            var layouts = await _deviceLayoutFiles.LayoutFiles();
            var selectedLayout = layouts.FirstOrDefault(x => x.MatchesPath(reposnse.Payload.Device.Layout));
            var model = new SectionViewModel
            {
                Record = reposnse.Payload,
                Descriptors = _modules.Descriptors(),
                AvailableSectionIdentifiers = selectedLayout != null ? selectedLayout.SectionIdentifiers : new string[0]
            };
            var device = PageDevice<Device>();
            device.Breadcrumbs.Add(new BreadcrumbLink("Devices", Url.Action("Home", "Devices", new { id = "" })));
            device.Breadcrumbs.Add(new BreadcrumbLink(model.Record.Device.FriendlyName, Url.Action("Home", "Devices", new { id = model.Record.Device.Id })));
            device.Breadcrumbs.Add(new BreadcrumbLink(model.Record.FriendlyName, Url.Action("Section", "Devices", new { id = model.Record.Id })));
            PageDevice(device);
            return View(model);
        }

        [HttpPost("section/edit")]
        public async Task<IActionResult> SectionEdit(SectionCreateOrUpdateCommand model)
        {
            var result = await Dispatcher.Send(model);
            if (ModelState.IsValid)
            {
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return RedirectToAction(nameof(Section), new { id = result.Payload.Id });
        }

        [HttpPost("section/delete")]
        public async Task<IActionResult> SectionDelete(SectionDeleteCommand model, string redirectUrl)
        {
            var result = await Dispatcher.Send(model);
            if (ModelState.IsValid)
            {
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return Redirect(redirectUrl);
        }

        [HttpGet("slot/{id}")]
        public async Task<IActionResult> Slot(int id)
        {
            var reposnse = await Dispatcher.Send(new SlotRequest(id));

            SetPageTitle($"Section: {reposnse.Payload.Record.Section.FriendlyName} Slot: {reposnse.Payload.Record.Module(reposnse.Payload.Record.Section.Identifier).renderer.FriendlyName}", $"Device: {reposnse.Payload.Record.Section.Device.FriendlyName}");

            var model = reposnse.Payload;
            var device = PageDevice<Device>();
            device.Breadcrumbs.Add(new BreadcrumbLink("Devices", Url.Action("Home", "Devices", new { id = "" })));
            device.Breadcrumbs.Add(new BreadcrumbLink(model.Record.Section.Device.FriendlyName, Url.Action("Home", "Devices", new { id = model.Record.Section.Device.Id })));
            device.Breadcrumbs.Add(new BreadcrumbLink(model.Record.Section.FriendlyName, Url.Action("Section", "Devices", new { id = model.Record.Section.Id })));
            device.Breadcrumbs.Add(new BreadcrumbLink(reposnse.Payload.Record.Module(reposnse.Payload.Record.Section.Identifier).renderer.FriendlyName, Url.Action("Slot", "Devices", new { id = model.Record.Id })));
            PageDevice(device);
            return View(model);
        }

        [HttpPost("slot/edit")]
        public async Task<IActionResult> SlotEdit(SlotCreateOrUpdateCommand model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await Dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
                else
                {
                    return RedirectToAction("Slot", new { id = result.Payload.Id });
                }
            }
            return Redirect(redirectUrl);
        }

        [HttpPost("slot/delete")]
        public async Task<IActionResult> Slotdelete(SlotDeleteCommand model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await Dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return Redirect(redirectUrl);
        }
    }
}
using Borg.Cms.Basic.Lib.Features.Device.Commands;
using Borg.Cms.Basic.Lib.Features.Device.Queries;
using Borg.Cms.Basic.Lib.Features.Device.ViewModels;
using Borg.MVC;
using Borg.MVC.BuildingBlocks.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Areas.Backoffice.Controllers
{
    [Route("[area]/Devices")]
    [Area("Backoffice")]
    [Authorize]
    public class DevicesController : BorgController
    {
        private readonly IMediator _dispatcher;
        private readonly IModuleDescriptorProvider _modules;
        private readonly IDeviceLayoutFileProvider _deviceLayoutFiles;

        public DevicesController(ILoggerFactory loggerFactory, IMediator dispatcher, IModuleDescriptorProvider modules, IDeviceLayoutFileProvider deviceLayoutFiles) : base(loggerFactory)
        {
            _dispatcher = dispatcher;
            _modules = modules;
            _deviceLayoutFiles = deviceLayoutFiles;
        }

        [HttpGet("")]
        public async Task<IActionResult> Home()
        {
            ViewBag.LayoutFiles = await _deviceLayoutFiles.LayoutFiles();
            var model = await _dispatcher.Send(new DevicesRequest());
            SetPageTitle("Devices", $"{model.Payload.Count()} templates");
            return View(model.Payload);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Home(int id)
        {
            ViewBag.LayoutFiles = await _deviceLayoutFiles.LayoutFiles();
            var model = await _dispatcher.Send(new DeviceRequest(id));
            SetPageTitle($"Device: {model.Payload.FriendlyName}", $"{model.Payload.Sections.Count} sections");
            return View("Device", model.Payload);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(DeviceCreateOrUpdateCommand model)
        {
            var result = await _dispatcher.Send(model);
            if (ModelState.IsValid)
            {
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return RedirectToAction(nameof(Home), new { id = result.Payload.Id });
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(DeviceDeleteCommand model)
        {
            var result = await _dispatcher.Send(model);
            if (ModelState.IsValid)
            {
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
            var reposnse = await _dispatcher.Send(new SectionRequest(id));
            SetPageTitle($"Section: {reposnse.Payload.FriendlyName}", $"Device: {reposnse.Payload.Device.FriendlyName}");
            var model = new SectionViewModel
            {
                Record = reposnse.Payload,
                Descriptors = _modules.Descriptors()
            };
            return View(model);
        }

        [HttpPost("section/edit")]
        public async Task<IActionResult> SectionEdit(SectionCreateOrUpdateCommand model)
        {
            var result = await _dispatcher.Send(model);
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
            var result = await _dispatcher.Send(model);
            if (ModelState.IsValid)
            {
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return Redirect(redirectUrl);
        }

        [HttpPost("slot/edit")]
        public async Task<IActionResult> SlotEdit(SlotCreateOrUpdateCommand model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return Redirect(redirectUrl);
        }

        [HttpPost("slot/delete")]
        public async Task<IActionResult> Slotdelete(SlotDeleteCommand model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return Redirect(redirectUrl);
        }
    }
}
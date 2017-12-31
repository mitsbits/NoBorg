using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.Device.Commands;
using Borg.Cms.Basic.Lib.Features.Device.Queries;
using Borg.MVC;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Areas.Backoffice.Controllers
{
    [Route("[area]/Devices")]
    [Area("Backoffice")][Authorize]
    public class DevicesController : BorgController
    {
        private readonly IMediator _dispatcher;
        public DevicesController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory)
        {
            _dispatcher = dispatcher;
        }
        [HttpGet("")]
        public async Task< IActionResult> Home()
        {
            var model = await _dispatcher.Send(new DevicesRequest());
            SetPageTitle("Devices", $"{model.Payload.Count()} templates");
            return View(model.Payload);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Home(int id)
        {
            var model = await _dispatcher.Send(new DeviceRequest(id));
            SetPageTitle($"Device: {model.Payload.FriendlyName}", $"{model.Payload.Sections.Count()} sections");
            return View("Device",model.Payload);
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
        [HttpGet("section/{id}")]
        public async Task<IActionResult> Section(int id)
        {
            var model = await _dispatcher.Send(new SectionRequest(id));
            SetPageTitle($"Section: {model.Payload.FriendlyName}", $"Device: {model.Payload.Device.FriendlyName}");
            return View( model.Payload);
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

        [HttpPost("slot/edit")]
        public async Task<IActionResult> SlotEdit(SlotCreateOrUpdateCommand model)
        {
            var result = await _dispatcher.Send(model);
            if (ModelState.IsValid)
            {
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return RedirectToAction(nameof(Section), new { id = result.Payload?.Section?.Id });

        }


    }
}

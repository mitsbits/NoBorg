using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.CMS.ConfigurationBlocks.Queries;
using Borg.MVC.Services.Breadcrumbs;
using Borg.Platform.EF.CMS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    public class ConfigurationBlocksController : BackofficeController
    {
        public ConfigurationBlocksController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
        {
        }

        public async Task<IActionResult> Index(string id)
        {
            var rows = await Dispatcher.Send(new ConfigurationBlocksRequest());
            Breadcrumbs(new BreadcrumbLink("Configutation Blocks", Url.Action(nameof(Index), new {id="", controller = "ConfigurationBlocks"})));
            if (id.IsNullOrWhiteSpace())
            {
                SetPageTitle("Configuration Blocks");
                return View(rows.Succeded ? rows.Payload : new ConfigurationBlockState[0]);
            }

            var hit = rows.Payload.First(x => x.Id == id);
            SetPageTitle($"Configuration Blocks: {hit.Display}");
            return View("Item", rows.Succeded ? rows.Payload : new ConfigurationBlockState[0]);
        }
    }
}

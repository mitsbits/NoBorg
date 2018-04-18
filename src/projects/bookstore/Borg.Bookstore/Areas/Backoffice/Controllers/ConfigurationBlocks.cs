//using Borg.Cms.Basic.Lib;
//using Borg.Cms.Basic.Lib.Features.CMS.ConfigurationBlocks.Commands;
//using Borg.Cms.Basic.Lib.Features.CMS.ConfigurationBlocks.Queries;
//using Borg.MVC.Services.Breadcrumbs;
//using Borg.Platform.EF.CMS;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Borg.Bookstore.Areas.Backoffice.Controllers;

//namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
//{
//    public class ConfigurationBlocksController : BackofficeController
//    {
//        public ConfigurationBlocksController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
//        {
//        }

//        public async Task<IActionResult> Index(string id)
//        {
//            var rows = await Dispatcher.Send(new ConfigurationBlocksRequest());
//            Breadcrumbs(new BreadcrumbLink("Configutation Blocks", Url.Action(nameof(Index), new { id = "", controller = "ConfigurationBlocks" })));
//            if (id.IsNullOrWhiteSpace())
//            {
//                SetPageTitle("Configuration Blocks");
//                return View(rows.Succeded ? rows.Payload : new ConfigurationBlockState[0]);
//            }

//            var hit = rows.Payload.First(x => x.Id == id);
//            Breadcrumbs(new BreadcrumbLink(hit.Display, Url.Action(nameof(Index), new { id })));
//            SetPageTitle($"Configuration Blocks: {hit.Display}", $"<i class='{FontAwesomeEnum.Cog.ParseClass(hit.IconClass)} fa-2x'></i>");
//            return View("Item", rows.Succeded ? rows.Payload : new ConfigurationBlockState[0]);
//        }

//        [HttpPost]
//        public async Task<IActionResult> BlockIcon(ConfigurationBlockIconCommand message, string redirecturl)
//        {
//            if (ModelState.IsValid)
//            {
//                var result = await Dispatcher.Send(message);
//                if (!result.Succeded)
//                {
//                    AddErrors(result);
//                }
//            }
//            return Redirect(redirecturl);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Block(dynamic config, string redirecturl, string type)
//        {
//            var t = Type.GetType(type);
//            Convert.ChangeType(config, t);

//            if (ModelState.IsValid)
//            {
//                //var result = await Dispatcher.Send(message);
//                //if (!result.Succeded)
//                //{
//                //    AddErrors(result);
//                //}
//            }
//            return Redirect(redirecturl);
//        }
//    }
//}
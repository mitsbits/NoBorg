using Borg.Cms.Basic.Lib.Features.CMS.Commands;
using Borg.Cms.Basic.Lib.Features.Navigation.Queries;
using Borg.Cms.Basic.Lib.Features.Navigation.ViewModels;
using Borg.Platform.EF.CMS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    [Route("[area]/Menus")]
    public class MenusController : BackofficeController
    {
        public MenusController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
        {
        }

        [HttpGet("{id?}")]
        public async Task<IActionResult> Home(string id, int? row)
        {
            SetPageTitle(string.IsNullOrWhiteSpace(id) ? "Navigational Menus" : $"Navigational Menus {id.ToUpper()}");
            if (!string.IsNullOrWhiteSpace(id))
            {
                var model = new MenuViewModel
                {
                    Group = id,
                    Records = string.IsNullOrWhiteSpace(id)
                        ? new NavigationItemState[0]
                        : (await Dispatcher.Send(new MenuGroupRecordsRequest(id))).Payload,
                    SelectedState = null
                };
                if (row.HasValue && model.Records.Any(x => x.Id == row))
                    model.SelectedState = model.Records.Single(x => x.Id == row);
                SetPageTitle($"Menu {id}", model.SelectedState != null ? model.SelectedState.Article.Title : "");
                return View(model);
            }
            return View();
        }

        //[Route("group/{id?}")]
        //public async Task<IActionResult> Group(string id, int? row)
        //{
        //    SetPageTitle($"Menu {id}");
        //    var model = new MenuViewModel
        //    {
        //        Group = id,
        //        Records = string.IsNullOrWhiteSpace(id)
        //            ? new NavigationItemRecord[0]
        //            : (await _dispatcher.Send(new MenuGroupRecordsRequest(id))).Payload,
        //        SelectedRecord = new NavigationItemRecord() { Group = id }
        //    };
        //    if (row.HasValue && model.Records.Any(x => x.Id == row))
        //        model.SelectedRecord = model.Records.Single(x => x.Id == row);
        //    SetPageTitle($"Menu {id}", model.SelectedRecord != null ? model.SelectedRecord.Display : "");
        //    return View(model);
        //}

        [HttpPost("")]
        public async Task<IActionResult> Item(NavigationItemStateCreateOrUpdateCommand model)
        {
            if (ModelState.IsValid)
            {
                var result = await Dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                    return RedirectToAction(nameof(Home), new { id = model.Group });
                }
                return RedirectToAction(nameof(Home), new { id = model.Group, row = result.Payload.Id });
            }
            return RedirectToAction(nameof(Home), new { id = model.Group });
        }

        [HttpGet("Delete/{id:alpha}/{row:int}")]
        public async Task<IActionResult> Delete(string id, int row)
        {
            var model = new NavigationItemStateDeleteCommand() { Group = id, RecordId = row };
            if (ModelState.IsValid)
            {
                var result = await Dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return RedirectToAction(nameof(Home), new { id = model.Group });
        }
    }
}
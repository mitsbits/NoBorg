using Borg.Cms.Basic.Lib.Features.Navigation;
using Borg.Cms.Basic.Lib.Features.Navigation.Commands;
using Borg.Cms.Basic.Lib.Features.Navigation.Queries;
using Borg.Cms.Basic.Lib.Features.Navigation.ViewModels;
using Borg.MVC;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Areas.Backoffice.Controllers
{
    [Route("[area]/Menus")]
    [Area("Backoffice")]
    [Authorize]
    public class MenusController : BorgController
    {
        private readonly IMediator _dispatcher;

        public MenusController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory)
        {
            _dispatcher = dispatcher;
        }

        public async Task<IActionResult> Home()
        {
            SetPageTitle("Navigational Menus Dashboards");
            var model = new MenusDashboardViewModel
            {
                Groups = (await _dispatcher.Send(new MenuGroupsRequest())).Payload.ToArray(),
            };

            return View(model);
        }

        [Route("group/{id?}")]
        public async Task<IActionResult> Group(string id, int? row)
        {
            SetPageTitle($"Menu {id}");
            var model = new MenuViewModel
            {
                Group = id,
                Records = string.IsNullOrWhiteSpace(id)
                    ? new NavigationItemRecord[0]
                    : (await _dispatcher.Send(new MenuGroupRecordsRequest(id))).Payload,
                SelectedRecord = new NavigationItemRecord() { Group = id }
            };
            if (row.HasValue && model.Records.Any(x => x.Id == row))
                model.SelectedRecord = model.Records.Single(x => x.Id == row);
            SetPageTitle($"Menu {id}", model.SelectedRecord != null ? model.SelectedRecord.Display : "");
            return View(model);
        }

        [HttpPost("")]
        public async Task<IActionResult> Item(NavigationItemRecordCreateOrUpdateCommand model)
        {
            if (ModelState.IsValid)
            {
                var result = await _dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
                else
                {
                    model.Record = result.Payload;
                }
            }
            return RedirectToAction(nameof(Group), new { id = model.Record.Group, row = model.Record.Id });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(NavigationItemRecordDeleteCommand model)
        {
            if (ModelState.IsValid)
            {
                var result = await _dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return RedirectToAction(nameof(Group), new { id = model.Group });
        }
    }
}
using Borg.Cms.Basic.Lib.Features.CMS.Categories.Commands;
using Borg.Cms.Basic.Lib.Features.CMS.Categories.Queries;
using Borg.Cms.Basic.Lib.Features.CMS.Categories.ViewModels;
using Borg.Infra.Collections;
using Borg.Platform.EF.CMS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    [Authorize(Policy = "ContentEditor")]
    [Route("[area]/Categories")]
    public class CategoriesController : BackofficeController
    {
        public CategoriesController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
        {
        }

        [HttpGet("")]
        public async Task<IActionResult> Home()
        {
            var result = await Dispatcher.Send(new CategoryGroupingIndexRequest());
            var model = new CategoryGroupingIndexViewModel()
            {
                Index = !result.Succeded ? new PagedResult<CategoryGroupingState>() : result.Payload
            };
            SetPageTitle("Categories");
            return View(model);
        }

        [HttpGet("{id:int}/{catid:int?}")]
        public async Task<IActionResult> Grouping(int id, int? catid)
        {
            var response = await Dispatcher.Send(new CategoryGroupingAggregateRequest(id));
            SetPageTitle(response.Payload.FriendlyName);
            var model = new CategoryGroupingViewModel()
            {
                AggregateRoot = response.Payload,
                SelectedCategoryId = catid.HasValue && catid.Value > 0 ? catid.Value : default(int)
            };

            return View(model);
        }

        [HttpPost("GroupingCommand")]
        public async Task<IActionResult> GroupingCommand(AddOrUpdateCategoryGroupingCommand model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                var isTransient = model.RecordId == default(int);
                var result = await Dispatcher.Send(model);
                if (result.Succeded && isTransient)
                {
                    return RedirectToAction(nameof(Grouping), new { id = result.Payload.Id });
                }
            }

            return RedirectToLocal(redirecturl);
        }
    }
}
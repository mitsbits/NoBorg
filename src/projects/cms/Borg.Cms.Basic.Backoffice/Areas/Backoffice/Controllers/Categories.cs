using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.CMS.Categories.Commands;
using Borg.Cms.Basic.Lib.Features.CMS.Categories.Queries;
using Borg.Cms.Basic.Lib.Features.CMS.Categories.ViewModels;
using Borg.Infra.Collections;
using Borg.Platform.EF.CMS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public async Task< IActionResult> Home()
        {
            var result = await Dispatcher.Send(new CategoryGroupingIndexRequest());
            var model = new CategoryGroupingIndexViewModel()
            {
                Index = !result.Succeded ? new PagedResult<CategoryGroupingState>() : result.Payload
            };
            SetPageTitle("Categories");
            return View(model);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Grouping(int id)
        {
            var response = await Dispatcher.Send(new CategoryGroupingAggregateRequest(id));
            SetPageTitle(response.Payload.FriendlyName);
            return View(response.Payload);
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
                    return RedirectToAction(nameof(Grouping), new {id = result.Payload.Id});
                }
            }

            return RedirectToLocal(redirecturl);
        }
    }
}
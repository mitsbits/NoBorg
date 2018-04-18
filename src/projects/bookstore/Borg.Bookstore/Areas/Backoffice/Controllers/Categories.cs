//using Borg.Cms.Basic.Lib.Features.CMS.Categories.Commands;
//using Borg.Cms.Basic.Lib.Features.CMS.Categories.Queries;
//using Borg.Cms.Basic.Lib.Features.CMS.Categories.ViewModels;
//using Borg.Infra.Collections;
//using Borg.Platform.EF.CMS;
//using MediatR;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System.Threading.Tasks;
//using Borg.Bookstore.Areas.Backoffice.Controllers;

//namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
//{
//    [Authorize(Policy = "ContentEditor")]
//    [Route("[area]/Categories")]
//    public class CategoriesController : BackofficeController
//    {
//        public CategoriesController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
//        {
//        }

//        [HttpGet("")]
//        public async Task<IActionResult> Home()
//        {
//            var result = await Dispatcher.Send(new CategoryGroupingIndexRequest());
//            var model = new CategoryGroupingIndexViewModel()
//            {
//                Index = !result.Succeded ? new PagedResult<CategoryGroupingState>() : result.Payload
//            };
//            SetPageTitle("Categories");
//            return View(model);
//        }

//        [HttpGet("{id:int}/{catid:int?}")]
//        public async Task<IActionResult> Grouping(int id, int? catid)
//        {
//            var response = await Dispatcher.Send(new CategoryGroupingAggregateRequest(id));
//            SetPageTitle(response.Payload.FriendlyName);
//            var model = new CategoryGroupingViewModel()
//            {
//                AggregateRoot = response.Payload,
//                SelectedCategoryId = catid.HasValue && catid.Value > 0 ? catid.Value : default(int)
//            };

//            return View(model);
//        }

//        [HttpPost("GroupingCommand")]
//        public async Task<IActionResult> GroupingCommand(AddOrUpdateCategoryGroupingCommand model, string redirecturl)
//        {
//            if (ModelState.IsValid)
//            {
//                var isTransient = model.RecordId == default(int);
//                var result = await Dispatcher.Send(model);
//                if (result.Succeded && isTransient)
//                {
//                    return RedirectToAction(nameof(Grouping), new { id = result.Payload.Id });
//                }
//            }

//            return RedirectToLocal(redirecturl);
//        }
//        [HttpPost("CategoryEdit")]
//        public async Task<IActionResult> CategoryEdit(CategoryEditViewModel model, string redirecturl)
//        {
//            if (ModelState.IsValid)
//            {
//                var command = new AddOrUpdateCategoryCommand() { ParentId = model.ParentId, RecordId = model.RecordId, Slug = model.Slug, FriendlyName = model.FriendlyName, UpdateSlugFromName = model.AlsoSetSlug, Weight = model.Weight, GroupingId = model.GroupingId};
//                var result = await Dispatcher.Send(command);
//                if (!result.Succeded)
//                {
//                    AddErrors(result);
//                    return RedirectToLocal(redirecturl);
//                }
//                return RedirectToAction("Grouping", new {id = model.GroupingId, catid = result.Payload.Id});

//            }
//            return RedirectToLocal(redirecturl);
//        }
//    }
//}
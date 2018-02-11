using System.Runtime.InteropServices.ComTypes;
using Borg.Cms.Basic.Backoffice.Areas.Backoffice.Components;
using Borg.Cms.Basic.Lib.Features.CMS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.CMS.Commands;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    public class TagsController : BackofficeController
    {
        public TagsController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
        {
        }

        [HttpGet]
        public async Task<IActionResult> TagOptions(string searchTerm, int? pageSize, int? pageNum)
        {
            pageSize = pageSize ?? 30;
            pageNum = pageNum ?? 1;
            var result = await Dispatcher.Send(new TagSuggestionRequest(searchTerm, pageNum.Value, pageSize.Value));
            return Json(new { Data = result });
        }

        [HttpPost]
        public async Task< IActionResult> AssociateAtricleTags(ArticleTagsAssociationViewModel model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await Dispatcher.Send(
                        new ArticleTagsAssociationCommand() {RecordId = model.ArticleId, Tags = model.Selection});
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return RedirectPermanent(redirecturl);
        }
    }
}
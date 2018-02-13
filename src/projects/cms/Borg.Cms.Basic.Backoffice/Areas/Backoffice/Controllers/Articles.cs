using Borg.Cms.Basic.Lib.Features.CMS.Commands;
using Borg.Cms.Basic.Lib.Features.CMS.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    [Authorize(Policy = "ContentEditor")]
    [Route("[area]/Articles")]
    public class ArticlesController : BackofficeController
    {
        public ArticlesController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
        {
        }

        [Route("{id:int}")]
        public async Task<IActionResult> Item(int id)
        {
            var result = await Dispatcher.Send(new ArticleRequest(id));
            if (result.Succeded)
            {
                SetPageTitle(result.Payload.Title);
                return View(result.Payload);
            }
            return View("404");
        }

        [HttpPost("Rename")]
        public async Task<IActionResult> Rename(RenameArticleCommand model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                var result = await Dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return RedirectToLocal(redirecturl);
        }

        [HttpPost("SetSlug")]
        public async Task<IActionResult> SetSlug(SetArticleSlugCommand model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                var result = await Dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return RedirectToLocal(redirecturl);
        }
    }
}
using Borg.Cms.Basic.Lib.Features.CMS.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.CMS.Commands;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    [Authorize(Policy = "ContentEditor")]
    [Route("[area]/HtmlSnippet")]
    public class HtmlSnippetController : BackofficeController
    {
        public HtmlSnippetController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
        {
        }

        public async Task<IActionResult> Home(int? id)
        {
            var model = new HtmlSnippetHomeViewModel();
            var indicesResponse = await Dispatcher.Send(new HtmlSnippetIndicesRequest());
            if (indicesResponse.Succeded) model.Indices = indicesResponse.Payload;
            if (id.HasValue)
            {
                var modelResponse = await Dispatcher.Send(new HtmlSnippetModelRequest(id.Value));
                if (modelResponse.Succeded) model.Snippet = modelResponse.Payload;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Home(AddOrUpdateHtmlSnippetCommand model, string redirecturl)
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

    public class HtmlSnippetHomeViewModel
    {
        public IEnumerable<HtmlSnippetIndex> Indices { get; set; } = new List<HtmlSnippetIndex>();
        public HtmlSnippetModel Snippet { get; set; } = new HtmlSnippetModel();
    }
}
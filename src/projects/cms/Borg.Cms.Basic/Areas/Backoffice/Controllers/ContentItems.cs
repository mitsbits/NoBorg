using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.Content.Commands;
using Borg.MVC;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Areas.Backoffice.Controllers
{
    [Route("[area]/Content")]
    [Area("Backoffice")]
    public class ContentItemsController : BorgController
    {
        private readonly IMediator _dispatcher;

        public ContentItemsController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet("{id:int}")]
        public IActionResult Item(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> Item(ContentItemCreateOrUpdateCommand model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                var result = await _dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                }
            }
            return RedirectToLocal(redirecturl);
        }
    }
}
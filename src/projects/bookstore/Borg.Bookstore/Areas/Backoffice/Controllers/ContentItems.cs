////using Borg.Cms.Basic.Lib.Features.Content.Commands;
////using MediatR;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.Extensions.Logging;
////using System.Threading.Tasks;
////using Borg.Bookstore.Areas.Backoffice.Controllers;

////namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
////{
////    [Route("[area]/Content")]
////    public class ContentItemsController : BackofficeController
////    {
////        [HttpGet("{id:int}")]
////        public IActionResult Item(int id)
////        {
////            return View();
////        }

////        [HttpPost]
////        public async Task<IActionResult> Item(ContentItemCreateOrUpdateCommand model, string redirecturl)
////        {
////            if (ModelState.IsValid)
////            {
////                var result = await Dispatcher.Send(model);
////                if (!result.Succeded)
////                {
////                    AddErrors(result);
////                }
////            }
////            return RedirectToLocal(redirecturl);
////        }

////        public ContentItemsController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
////        {
////        }
////    }
////}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.CMS.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    [Authorize(Policy = "ContentEditor")]
    [Route("Articles")]
    public class ArticlesController : BackofficeController
    {
        public ArticlesController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
        {
        }

        [Route("{id:int}")]
        public async Task< IActionResult > Item(int id)
        {
            var result = await Dispatcher.Send(new ArticleRequest(id));
            if (result.Succeded) return View(result.Payload);
            return View();
        }
    }
}

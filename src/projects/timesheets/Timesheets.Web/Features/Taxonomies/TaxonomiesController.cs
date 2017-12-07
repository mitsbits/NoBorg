using Borg.Infra.DAL;
using Borg.Infra.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Timesheets.Web.Features.Taxonomies.Requests;
using Timesheets.Web.Features.Taxonomies.Services;
using Timesheets.Web.Infrastructure;

namespace Timesheets.Web.Features.Taxonomies
{
    [Route("Taxonomies")]
    public class TaxonomiesController : FrameworkController
    {
        private readonly IMediator _dispatcher;

        public TaxonomiesController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory)
        {
            _dispatcher = dispatcher;
        }

        [Route("{id?}")]
        public async Task<IActionResult> Taxonomy([FromServices]ITaxonomyService service, string id)
        {
            Tidings tidings = null;
            TaxonomyViewModel model = null;

            var result = await _dispatcher.Send(new TaxonomyRequest(string.IsNullOrWhiteSpace(id) ? default(Guid?) : Guid.Parse(id)));
            if (result.Succeded)
            {
                model = (result as QueryResult<TaxonomyViewModel>)?.Payload;
                model.Options = (await service.Tree()).Clone();
            }

            SetPageTitle(string.IsNullOrWhiteSpace(model.Source.DisplayName) ? "New Taxonomy" : model.Source.DisplayName);
            return View(model);
        }
    }
}
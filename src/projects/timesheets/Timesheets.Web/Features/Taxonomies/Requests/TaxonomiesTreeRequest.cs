using Borg.Infra.DAL;
using Borg.Infra.DTO;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Timesheets.Web.Domain;
using Timesheets.Web.Features.Taxonomies.Services;

namespace Timesheets.Web.Features.Taxonomies.Requests
{
    public class TaxonomiesTreeRequest : IRequest<QueryResult>
    {
    }

    public class TaxonomiesTreeRequestHandler : IAsyncRequestHandler<TaxonomiesTreeRequest, QueryResult>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<TimesheetsDbContext> _uow;
        private readonly ITaxonomyService _service;

        public TaxonomiesTreeRequestHandler(ILoggerFactory loggerfactory, IUnitOfWork<TimesheetsDbContext> uow, ITaxonomyService service)
        {
            _logger = loggerfactory.CreateLogger(typeof(Workers.WorkingDaysRequestHandler));
            _uow = uow;
            _service = service;
        }

        public async Task<QueryResult> Handle(TaxonomiesTreeRequest message)
        {
            try
            {
                var taxonomies = await _uow.QueryRepo<Taxonomy>().Find(x => true, SortBuilder.Get<Taxonomy>().Build(), CancellationToken.None, taxonomy => taxonomy.Tags);

                Tidings tree = await _service.Tree(taxonomies.Records);

                return QueryResult<Tidings>.Success(tree);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Handler: @handler", GetType());
                return QueryResult.Failure(ex.ToString());
            }
        }
    }
}
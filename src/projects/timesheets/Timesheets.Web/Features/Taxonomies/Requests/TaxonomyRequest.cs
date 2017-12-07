using Borg;
using Borg.Infra.DAL;
using Borg.Infra.DTO;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Web.Domain;

namespace Timesheets.Web.Features.Taxonomies.Requests
{
    public class TaxonomyViewModel
    {
        public Tidings Options { get; set; }
        public Taxonomy Source { get; set; }
        public IEnumerable<Tag> InheritedTags { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }

    public class TaxonomyRequest : IRequest<QueryResult>
    {
        public TaxonomyRequest(Guid? id)
        {
            Id = id;
        }

        public Guid? Id { get; }
    }

    public class TaxonomyRequestHandler : IAsyncRequestHandler<TaxonomyRequest, QueryResult>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<TimesheetsDbContext> _uow;

        public TaxonomyRequestHandler(ILoggerFactory loggerfactory, IUnitOfWork<TimesheetsDbContext> uow)
        {
            _logger = loggerfactory.CreateLogger(typeof(Workers.WorkingDaysRequestHandler));
            _uow = uow;
        }

        public async Task<QueryResult> Handle(TaxonomyRequest message)
        {
            try
            {
                TaxonomyViewModel model = new TaxonomyViewModel();
                Taxonomy source = null;
                IEnumerable<Tag> inheritedTags = null;
                IEnumerable<Tag> tags = null;

                var task1 = Task.Run(async () =>
               {
                   if (message.Id.HasValue && message.Id.Value != Guid.Empty)
                   {
                       source = await _uow.QueryRepo<Taxonomy>().Get(x => x.Id.Equals(message.Id.Value));
                   }
                   else
                   {
                       source = new Taxonomy(null, string.Empty);
                   }
               });

                var task2 = Task.Run(async () =>
                {
                    var localtags = new List<Tag>();
                    var current = (await _uow.Context.Taxonomies.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(message.Id)))?.ParentId;
                    while (current.HasValue)
                    {
                        var tagsQuery = from tag in _uow.Context.Tags
                                        join taxonomyTag in _uow.Context.TaxonomiesTags on tag.Id equals taxonomyTag.TagId
                                        join taxonomy in _uow.Context.Taxonomies on taxonomyTag.TaxonomyId equals taxonomy.Id
                                        where taxonomy.Id.Equals(current.Value)
                                        select tag;
                        localtags.AddRange(await tagsQuery.ToListAsync());
                        var current1 = current;
                        current = (await _uow.Context.Taxonomies.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(current1)))?.ParentId;
                    }
                    inheritedTags = localtags;
                });

                var task3 = Task.Run(async () =>
                {
                    var localcurrenttags = new List<Tag>();

                    var currenttagsQuery = from tag in _uow.Context.Tags
                                           join taxonomyTag in _uow.Context.TaxonomiesTags on tag.Id equals taxonomyTag.TagId
                                           join taxonomy in _uow.Context.Taxonomies on taxonomyTag.TaxonomyId equals taxonomy.Id
                                           where taxonomy.Id.Equals(message.Id)
                                           select tag;
                    localcurrenttags.AddRange(await currenttagsQuery.ToListAsync());
                    tags = localcurrenttags;
                });

                await Task.WhenAll(task1, task2, task3).AnyContext();
                //await Task.WhenAll(task1).AnyContext();
                //await Task.WhenAll(task2).AnyContext();
                //await Task.WhenAll(task3).AnyContext();

                model.Source = source;
                model.InheritedTags = inheritedTags;
                model.Tags = tags;
                return QueryResult<TaxonomyViewModel>.Success(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Handler: @handler", GetType());
                return QueryResult.Failure(ex.ToString());
            }
        }
    }
}
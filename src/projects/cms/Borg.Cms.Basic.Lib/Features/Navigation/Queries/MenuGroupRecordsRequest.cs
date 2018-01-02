using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Navigation.Queries
{
    public class MenuGroupRecordsRequest : IRequest<QueryResult<IEnumerable<NavigationItemRecord>>>
    {
        public MenuGroupRecordsRequest(string @group, bool excludeSupressed = false)
        {
            if (@group.Length != 3) throw new ArgumentException(nameof(@group));
            Group = @group;
            ExcludeSupressed = excludeSupressed;
        }

        public string Group { get; }
        public bool ExcludeSupressed { get; } = false;
    }

    public class MenuGroupRecordsRequestHandler : AsyncRequestHandler<MenuGroupRecordsRequest, QueryResult<IEnumerable<NavigationItemRecord>>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<BorgDbContext> _uow;

        public MenuGroupRecordsRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<IEnumerable<NavigationItemRecord>>> HandleCore(MenuGroupRecordsRequest message)
        {
            var repo = _uow.QueryRepo<NavigationItemRecord>();
            var bldr = SortBuilder.Get<NavigationItemRecord>();
            bldr.Add(x => x.Weight, false).Add(x => x.ParentId);
            var results = message.ExcludeSupressed ? await repo.Find(x => x.Group == message.Group && x.IsPublished, bldr.Build()) : await repo.Find(x => x.Group == message.Group, bldr.Build());
            return QueryResult<IEnumerable<NavigationItemRecord>>.Success(results);
        }
    }
}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Borg.Cms.Basic.Lib.Features.CMS.Queries;
//using Borg.Cms.Basic.Lib.Features.Navigation.Services;
//using Borg.Infra.DAL;
//using Borg.Platform.EF.CMS.Data;
//using Borg.Platform.EF.Contracts;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace Borg.Cms.Basic.Lib.Features.Navigation.Queries
//{
//   public class GroupMenuContainerRequest : IRequest<QueryResult<MenuContainer>>
//    {
//        public GroupMenuContainerRequest(string @group, bool suppressInactive)
//        {
//            Group = @group;
//            SuppressInactive = suppressInactive;
//        }

//        public string Group { get; }
//        public bool SuppressInactive { get; }
//    }

//    public class GroupMenuContainerRequestHandler : AsyncRequestHandler<GroupMenuContainerRequest, QueryResult<MenuContainer>>
//    {
//        private readonly ILogger _logger;
//        private readonly IUnitOfWork<CmsDbContext> _uow;

//        public GroupMenuContainerRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
//        {
//            _logger = loggerFactory.CreateLogger(GetType());
//            _uow = uow;
//        }

//        protected override async Task<QueryResult<MenuContainer>> HandleCore(GroupMenuContainerRequest message)
//        {
//            try
//            {
//                var result = await _uow.Context.HtmlSnippetStates.Include(x => x.Component)
//                    .AsNoTracking()
//                    .Select(x => new HtmlSnippetIndex()
//                    {
//                        Id = x.Id,
//                        Code = x.Code,
//                        IsDeleted = x.Component.IsDeleted,
//                        IsPublished = x.Component.IsPublished
//                    }).ToListAsync();

//                return QueryResult<IEnumerable<HtmlSnippetIndex>>.Success(result);
//            }
//            catch (Exception e)
//            {
//                _logger.Error(e);
//                return QueryResult<IEnumerable<HtmlSnippetIndex>>.Failure(e.Message);
//            }
//        }
//    }
//}
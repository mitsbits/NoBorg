using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Borg.Cms.Basic.PlugIns.Documents.Data;
using Borg.Cms.Basic.PlugIns.Documents.ViewModels;
using Borg.Infra.Collections;
using Borg.Infra.DAL;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.PlugIns.Documents.Queries
{

    public class MimeTypeGroupingRowsRequest : IRequest<QueryResult<IPagedResult<MimeTypeGroupimgRowViewModel>>>
    {
        public MimeTypeGroupingRowsRequest()
        {

        }


    }

    public class MimeTypeGroupingRowsRequestHandler : AsyncRequestHandler<MimeTypeGroupingRowsRequest, QueryResult<IPagedResult<MimeTypeGroupimgRowViewModel>>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<DocumentsDbContext> _uow;


        public MimeTypeGroupingRowsRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<DocumentsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;

        }

        protected override async Task<QueryResult<IPagedResult<MimeTypeGroupimgRowViewModel>>> HandleCore(MimeTypeGroupingRowsRequest message)
        {
            try
            {
                var bucket = new List<MimeTypeGroupimgRowViewModel>();
                var conn = _uow.Context.Database.GetDbConnection();
                using (var sqlcom = conn.CreateCommand())
                {
                    sqlcom.CommandText = @"   SELECT grp.[Id], grp.[Name], grp.[Description], ExtCount = sub.eCount 
                                              FROM [documents].[MimeTypeGroupingStates] grp
                                              OUTER APPLY (   SELECT g.Id, eCount = Count(e.Extension) 
				                                              FROM [documents].[MimeTypeGroupingStates] g 
				                                              INNER JOIN [documents].[MimeTypeGroupingExtensionStates] e 
				                                              ON g.Id = e.MimeTypeGroupingId
				                                              WHERE grp.[Id] = g.Id
				                                              GROUP BY g.Id) sub";
                    if (sqlcom.Connection.State == ConnectionState.Closed) await sqlcom.Connection.OpenAsync();

                    using (var rdr = await sqlcom.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            var row = new MimeTypeGroupimgRowViewModel()
                            {
                                RecordId = rdr.GetInt32(0),
                                Name = rdr.GetString(1),
                                Description = rdr.GetString(2),
                                MimeTypesCount = rdr.GetInt32(3)
                            };
                            bucket.Add(row);
                        }
                    }
                }

                var result = new PagedResult<MimeTypeGroupimgRowViewModel>(bucket, 1, bucket.Count, bucket.Count);
                return QueryResult<IPagedResult<MimeTypeGroupimgRowViewModel>>.Success(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<IPagedResult<MimeTypeGroupimgRowViewModel>>.Failure(e.Message);
            }
        }
    }
}

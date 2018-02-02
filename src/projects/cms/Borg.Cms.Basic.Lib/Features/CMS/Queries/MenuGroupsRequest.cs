using Borg.Infra.DAL;
using Borg.Platform.EF.CMS.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Queries
{
    public class MenuGroupsRequest : IRequest<QueryResult<IEnumerable<string>>>
    {
    }

    public class MenuGroupsRequestHandler : AsyncRequestHandler<MenuGroupsRequest, QueryResult<IEnumerable<string>>>
    {
        private readonly ILogger _logger;
        private readonly CmsDbContext _db;

        public MenuGroupsRequestHandler(ILoggerFactory loggerFactory, CmsDbContext db)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _db = db;
        }

        protected override async Task<QueryResult<IEnumerable<string>>> HandleCore(MenuGroupsRequest request)
        {
            var conn = _db.Database.GetDbConnection();
            var model = new List<string>();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT [GroupCode]  "
                                   + "FROM [cms].[NavigationItemStates] "
                                   + "GROUP BY [GroupCode]";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            model.Add(reader.GetString(0));
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }
            return QueryResult<IEnumerable<string>>.Success(model);
        }
    }
}
using System.Data;
using Borg.Cms.Basic.Lib.Features.Device.Queries;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Device.Services
{
    public class DeviceStructureProvider : IDeviceStructureProvider
    {
        private readonly IMediator _dispatcher;
        private readonly IUnitOfWork<BorgDbContext> _uow;

        public DeviceStructureProvider(IMediator dispatcher, IUnitOfWork<BorgDbContext> uow)
        {
            _dispatcher = dispatcher;
            _uow = uow;
        }

        public async Task<IDeviceStructureInfo> PageLayout(int id)
        {
            var result = await _dispatcher.Send(new PageLayoutRequest(id));
            return result.Payload;
        }

        public async Task<IDeviceStructureInfo> PageLayout(string layout)
        {
            Preconditions.NotEmpty(layout, nameof(layout));
            int id = -1;
            var connection = _uow.Context.Database.GetDbConnection();

            using (var command = connection.CreateCommand())
            {
                if (connection.State == ConnectionState.Closed) await connection.OpenAsync();
                var param = new SqlParameter("@layout", layout);
                command.Parameters.Add(param);
                command.CommandText = "SELECT TOP 1 [Id] FROM [borg].[DeviceRecords] WHERE [Layout] = @layout";
                id = (int)await command.ExecuteScalarAsync();
            }
            if (id > 0)
            {
                return await PageLayout(id);
            }

            return null;
        }
    }
}
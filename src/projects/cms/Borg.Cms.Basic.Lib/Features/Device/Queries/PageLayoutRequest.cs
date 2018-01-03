using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Device.Queries
{
    public class PageLayoutRequest : IRequest<QueryResult<IDeviceStructureInfo>>
    {
        public PageLayoutRequest(int recordId)
        {
            RecordId = recordId;
        }

        public int RecordId { get; }
    }

    public class PageLayoutRequestHandler : AsyncRequestHandler<PageLayoutRequest, QueryResult<IDeviceStructureInfo>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<BorgDbContext> _uow;

        public PageLayoutRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<IDeviceStructureInfo>> HandleCore(PageLayoutRequest message)
        {
            var hit = await _uow.Context.DeviceRecords.Include(x => x.Sections).ThenInclude(x => x.Slots)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == message.RecordId);
            if (hit == null) return QueryResult<IDeviceStructureInfo>.Failure($"No device for id {message.RecordId}");

            var result = new DeviceStructureInfo()
            {
                RenderScheme = hit.RenderScheme,
                Layout = hit.Layout
            };
            foreach (var sectionRecord in hit.Sections)
            {
                var section = new Section()
                {
                    FriendlyName = sectionRecord.FriendlyName,
                    Identifier = sectionRecord.Identifier,
                    RenderScheme = sectionRecord.RenderScheme
                };
                foreach (var slotRecord in sectionRecord.Slots)
                {
                    try
                    {
                        var values = slotRecord.Module(sectionRecord.Identifier);           
                        section.DefineSlot(values.slotInfo, values.renderer);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                result.Sections.Add(section);
            }
            return QueryResult<IDeviceStructureInfo>.Success(result);
        }
    }
}
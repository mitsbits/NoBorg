﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Borg.Cms.Basic.Lib.Features.Device.Queries
{
    public class PageLayoutRequest : IRequest<QueryResult<IDeviceStrctureInfo>>
    {
        public PageLayoutRequest(int recordId)
        {
            RecordId = recordId;
        }

        public int RecordId { get; }
    }


    public class PageLayoutRequestHandler : AsyncRequestHandler<PageLayoutRequest, QueryResult<IDeviceStrctureInfo>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<BorgDbContext> _uow;

        public PageLayoutRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<IDeviceStrctureInfo>> HandleCore(PageLayoutRequest message)
        {
            var hit = await _uow.Context.DeviceRecords.Include(x => x.Sections).ThenInclude(x => x.Slots)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == message.RecordId);
            if (hit == null) return QueryResult<IDeviceStrctureInfo>.Failure($"No device for id {message.RecordId}");

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
                        var renderer = JsonConvert.DeserializeObject<ModuleRenderer>(slotRecord.ModuleDecriptorJson);
                        var slot = new SectionSlotInfo(sectionRecord.Identifier, slotRecord.IsEnabled, slotRecord.Ordinal);
                        section.DefineSlot(slot, renderer);
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                result.Sections.Add(section);
            }
            return QueryResult<IDeviceStrctureInfo>.Success(result);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.Device.Queries;
using Borg.MVC.BuildingBlocks.Contracts;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.Device.Services
{
 public   class DeviceStructureProvider: IDeviceStructureProvider
    {
        private readonly IMediator _dispatcher;

        public DeviceStructureProvider(IMediator dispatcher)
        {
            _dispatcher = dispatcher;
        }




        public async Task<IDeviceStructureInfo> PageLayout(int id)
        {
            var result = await _dispatcher.Send(new PageLayoutRequest(id));
            return result.Payload;
        }
    }
}

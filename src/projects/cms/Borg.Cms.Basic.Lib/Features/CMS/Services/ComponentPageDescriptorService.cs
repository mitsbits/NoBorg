using Borg.Cms.Basic.Lib.Features.CMS.Queries;
using Borg.Infra.Caching.Contracts;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Borg.MVC.PlugIns.Decoration;

namespace Borg.Cms.Basic.Lib.Features.CMS.Services
{
    [SingletonService(typeof(IComponentPageDescriptorService<int>))]
    public class ComponentPageDescriptorService : IComponentPageDescriptorService<int>
    {
        private readonly ILogger _logger;
        private readonly ICacheStore _cache;
        private readonly IMediator _dispatcher;

        public ComponentPageDescriptorService(ILoggerFactory loggerFactory, ICacheStore cache, IMediator dispatcher)
        {
            _cache = cache;
            _dispatcher = dispatcher;
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }

    
        public async Task<ComponentPageDescriptor<int>> Get(int componentId)
        {
            var cachehit = await _cache.Get<ComponentPageDescriptor<int>>(KeyFactory(componentId));
            if (cachehit == null)
            {
                cachehit = await FromDatabase(componentId);
                if (cachehit != null)
                {
                    await _cache.SetForEver(KeyFactory(componentId), cachehit);
                }else
                {
                 _logger.Warn("the requested component with key {id} is not present", componentId);   
                }
            }
            return cachehit;
        }

        public async Task<ComponentPageDescriptor<int>> Set(ComponentPageDescriptor<int> descriptor)
        {
            await _cache.SetForEver(KeyFactory(descriptor.ComponentId), descriptor);
            return descriptor;
        }

        public async Task Invalidate(int componentId)
        {
            await _cache.Remove(KeyFactory(componentId));
        }

        private string KeyFactory(int componentId)
        {
            return $"CK:{componentId}";
        }

        private async Task<ComponentPageDescriptor<int>> FromDatabase(int componentId)
        {
            var page = await _dispatcher.Send(new ComponentPageContentRequest(componentId));
            var device = await _dispatcher.Send(new ComponentDeviceRequest(componentId));
            if (page.Succeded && device.Succeded)
            {
                return new ComponentPageDescriptor<int>
                {
                    ComponentId = componentId,
                    Device = device.Payload,
                    PageContent = page.Payload.content
                };
            }
            return null;
        }
    }
}
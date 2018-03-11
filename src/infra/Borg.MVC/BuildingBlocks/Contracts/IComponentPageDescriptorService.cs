using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IComponentPageDescriptorService<TKey> where TKey : IEquatable<TKey>
    {
        Task<ComponentPageDescriptor<TKey>> Get(TKey componentId);
        Task<ComponentPageDescriptor<TKey>> Set(ComponentPageDescriptor<TKey> descriptor);
        Task Invalidate(TKey componentId);
    }
}

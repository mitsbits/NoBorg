using System.Collections.Generic;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IModuleDescriptorProvider
    {
        IEnumerable<IModuleDescriptor> Descriptors();
    }
}
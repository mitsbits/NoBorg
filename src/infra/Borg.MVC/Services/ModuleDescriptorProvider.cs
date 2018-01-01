using System.Collections.Generic;
using Borg.MVC.BuildingBlocks.Contracts;

namespace Borg.MVC.Services
{
    public class ModuleDescriptorProvider: IModuleDescriptorProvider
    {
        private readonly IEnumerable<IModuleDescriptor> _descriptors;

        public ModuleDescriptorProvider(IEnumerable<IModuleDescriptor> descriptors)
        {
            _descriptors = descriptors;
        }

        public IEnumerable<IModuleDescriptor> Descriptors() => _descriptors;
    }
}
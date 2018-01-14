using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Services.Editors;
using System.Collections.Generic;

namespace Borg.MVC.Services
{
    public class ModuleDescriptorProvider : IModuleDescriptorProvider
    {
        private readonly IEnumerable<IModuleDescriptor> _descriptors;
        private readonly IEnumerable<IEditorDescriptor> _editors;

        public ModuleDescriptorProvider(IEnumerable<IModuleDescriptor> descriptors, IEnumerable<IEditorDescriptor> editors)
        {
            _descriptors = descriptors;
            _editors = editors;
        }

        public IEnumerable<IModuleDescriptor> Descriptors() => _descriptors;

        public IEnumerable<IEditorDescriptor> Editors() => _editors;
    }
}
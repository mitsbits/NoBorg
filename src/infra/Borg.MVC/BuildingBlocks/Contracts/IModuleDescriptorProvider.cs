using Borg.MVC.Services.Editors;
using System.Collections.Generic;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IModuleDescriptorProvider
    {
        IEnumerable<IModuleDescriptor> Descriptors();

        IEnumerable<IEditorDescriptor> Editors();
    }
}
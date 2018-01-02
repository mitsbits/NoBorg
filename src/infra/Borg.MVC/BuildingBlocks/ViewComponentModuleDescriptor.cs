using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks.Contracts;

namespace Borg.MVC.BuildingBlocks
{
    public abstract class ViewComponentModuleDescriptor : ModuleDescriptorBase, IModuleDescriptor<ViewComponentModule<Tidings>, Tidings>
    {
        public override ModuleGender ModuleGender => ModuleGender.ViewComponent;
    }
}
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks.Contracts;

namespace Borg.MVC.BuildingBlocks
{
    public abstract class ViewModuleDescriptor : ModuleDescriptorBase, IModuleDescriptor<Tidings>
    {
        public override ModuleGender ModuleGender => ModuleGender.PartialView;
    }
}
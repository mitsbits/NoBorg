using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Borg.Cms.Basic.Presentation.Areas.Presentation.Modules
{
    public sealed class BodyViewModuleDescriptor : ViewModuleDescriptor
    {
        public override string FriendlyName => "Body View";
        public override string Summary => "Body View Description";
        public override string ModuleGroup => "System.Content";

        protected override Tidings GetDefaults()
        {
            var result = new Tidings
            {
                new Tiding(Tidings.DefinedKeys.AssemblyQualifiedName, typeof(RazorView).AssemblyQualifiedName),
                new Tiding(Tidings.DefinedKeys.View, "Modules/Body")
            };
            return result;
        }
    }

    public sealed class PrimaryImageViewModuleDescriptor : ViewModuleDescriptor
    {
        public override string FriendlyName => "Primary Image View";
        public override string Summary => "Primary Image View Description";
        public override string ModuleGroup => "System.Content";

        protected override Tidings GetDefaults()
        {
            var result = new Tidings
            {
                new Tiding(Tidings.DefinedKeys.AssemblyQualifiedName, typeof(RazorView).AssemblyQualifiedName),
                new Tiding(Tidings.DefinedKeys.View, "Modules/PrimaryImage"),
                new Tiding(Tidings.DefinedKeys.Size, "Large")
            };
            return result;
        }
    }
}
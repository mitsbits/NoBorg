using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Borg.Cms.Basic.Presentation.Areas.Presentation.Modules
{
    public sealed class FooterViewModuleDescriptor : ViewModuleDescriptor
    {
        public override string FriendlyName => "Footer View";
        public override string Summary => "Footer View Description";
        public override string ModuleGroup => "System.Content";

        protected override Tidings GetDefaults()
        {
            var result = new Tidings
            {
                new Tiding(Tidings.DefinedKeys.AssemblyQualifiedName, typeof(RazorView).AssemblyQualifiedName),
                new Tiding(Tidings.DefinedKeys.View, "Modules/Footer")
            };
            return result;
        }
    }
}
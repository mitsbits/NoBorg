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
                new Tiding("AssemblyQualifiedName", typeof(RazorView).AssemblyQualifiedName),
                new Tiding("view", "Shared/Modules/Body.cshtml")
            };
            return result;
        }
    }
}
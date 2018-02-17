using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Borg.Cms.Basic.Presentation.Areas.Presentation.Modules
{
    public sealed class TitleViewModuleDescriptor : ViewModuleDescriptor
    {
        public override string FriendlyName => "Title View";
        public override string Summary => "Title View Description";
        public override string ModuleGroup => "System.Content";

        protected override Tidings GetDefaults()
        {
            var result = new Tidings
            {
                new Tiding("AssemblyQualifiedName", typeof(RazorView).AssemblyQualifiedName),
                new Tiding("view", "Shared/Modules/Title.cshtml")
            };
            return result;
        }
    }
}
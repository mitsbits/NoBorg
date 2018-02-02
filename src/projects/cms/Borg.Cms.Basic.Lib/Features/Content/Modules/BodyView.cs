using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Borg.Cms.Basic.Lib.Features.Content.Modules
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
                new Tiding("optional-parameter", "Hello"),
                new Tiding("view", "~/Views/Shared/Modules/Body.cshtml")
            };
            return result;
        }
    }
}
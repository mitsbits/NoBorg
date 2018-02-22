using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Borg.Cms.Basic.Presentation.Areas.Presentation.Modules
{
    public class GoogleAnalyticsTrackingModuleDescriptor : ViewModuleDescriptor
    {
        public override string FriendlyName => "Google Analytics Tracking";
        public override string Summary => "GoogleAnalyticsTracking Description";
        public override string ModuleGroup => "System.Analytics";

        protected override Tidings GetDefaults()
        {
            var result = new Tidings
            {
                new Tiding(Tidings.DefinedKeys.AssemblyQualifiedName, typeof(RazorView).AssemblyQualifiedName),
                new Tiding(Tidings.DefinedKeys.View, "Modules/GoogleAnalyticsTracking")
            };
            return result;
        }
    }
}
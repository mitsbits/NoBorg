using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Borg.Cms.Basic.Presentation.Areas.Presentation.Modules
{
    public class TagsView : ViewModuleDescriptor
    {
        public override string FriendlyName => "Tags View";
        public override string Summary => "Tags View Description";
        public override string ModuleGroup => "System.Content";

        protected override Tidings GetDefaults()
        {
            var result = new Tidings
            {
                new Tiding(Tidings.DefinedKeys.AssemblyQualifiedName, typeof(RazorView).AssemblyQualifiedName),
                new Tiding(Tidings.DefinedKeys.View, "Modules/Tags")
            };
            return result;
        }
    }
}

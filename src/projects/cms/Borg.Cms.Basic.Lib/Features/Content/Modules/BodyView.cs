using System;
using System.Collections.Generic;
using System.Text;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Borg.Cms.Basic.Lib.Features.Content.Modules
{
    public sealed class BodyViewModuleDescriptor : IModuleDescriptor< Tidings>
    {
        public string FriendlyName => "Body View";
        public string Summary => "Body View Description";
        public string ModuleGroup => "System.Content";
        public ModuleGender ModuleGender => ModuleGender.PartialView;
        public Tidings Parameters => GetDefaults();

        private static Tidings GetDefaults()
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

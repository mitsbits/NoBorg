using System.Collections.Generic;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.Navigation.Contracts;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Borg.Cms.Basic.Lib.Features.Navigation.Modules
{
    public class MenuViewComponent : ViewComponentModule<Tidings>
    {
        private readonly IMenuProvider _menuProvider;

        private const string _viewKey = "view";
        private const string _groupKey = "group";

        public MenuViewComponent(IMenuProvider menuProvider)
        {
            _menuProvider = menuProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync(Tidings tidings)
        {
            var tree = await _menuProvider.Tree(tidings[_groupKey]);
            if (tidings.ContainsKey(_viewKey) && !string.IsNullOrWhiteSpace(tidings[_viewKey])) return View(tidings[_viewKey], tree);
            return View(tree);
        }
    }

    public sealed class MenuModuleDescriptor : IModuleDescriptor<MenuViewComponent,Tidings>
    {
        public string FriendlyName => "Navigation Menu";
        public string Summary => "Navigation Menu Description";
        public string ModuleGroup => "System.Navigation";
        public ModuleGender ModuleGender => ModuleGender.ViewComponent;
        public Tidings Parameters => GetDefaults();

        private static Tidings GetDefaults()
        {
            var result = new Tidings
            {
                new Tiding("AssemblyQualifiedName", typeof(MenuViewComponent).AssemblyQualifiedName),
                new Tiding("group", ""),
                new Tiding("view", "")
            };
            return result;
        }
    }



}
using Borg.Cms.Basic.Lib.Features.Navigation.Contracts;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Navigation.Modules
{
    public class MenuViewComponent : ViewComponentModule<Tidings>
    {
        private readonly IMenuProvider _menuProvider;

        private const string _groupKey = "group";

        public MenuViewComponent(IMenuProvider menuProvider)
        {
            _menuProvider = menuProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync(Tidings tidings)
        {
            try
            {
                var tree = await _menuProvider.Tree(tidings[_groupKey]);
                if (tidings.ContainsKey(Tidings.DefinedKeys.View) &&
                    !string.IsNullOrWhiteSpace(tidings[Tidings.DefinedKeys.View]))
                    return View(tidings[Tidings.DefinedKeys.View], tree);
                return View(tree);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public sealed class MenuModuleDescriptor : ViewComponentModuleDescriptor
    {
        public override string FriendlyName => "Navigation Menu";
        public override string Summary => "Navigation Menu Description";
        public override string ModuleGroup => "System.Navigation";

        protected override Tidings GetDefaults()
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
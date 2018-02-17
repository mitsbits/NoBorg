using System;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.Navigation.Contracts;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Presentation.Areas.Presentation.Components
{
    [PulgInViewComponent("Menu")]
    public class MenuViewComponent : ViewComponentModule<Tidings>
    {
        private readonly IMenuProvider _menuProvider;

        private const string _groupKey = "group";
        private readonly ILogger _logger;

        public MenuViewComponent(IMenuProvider menuProvider, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
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
                _logger.Error(ex);
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
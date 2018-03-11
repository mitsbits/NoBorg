using Borg.Cms.Basic.Lib.Features.Navigation.Contracts;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Borg.Cms.Basic.Presentation.Services.Contracts;
using Borg;
namespace Borg.Cms.Basic.Presentation.Areas.Presentation.Components
{
    [PulgInViewComponent("Menu")]
    public class MenuViewComponent : ViewComponentModule<Tidings>
    {
        private readonly IMenuProvider _menuProvider;

    
        private readonly ILogger _logger;

        private readonly IEntityMemoryStore _memoryStore;

        public MenuViewComponent(IMenuProvider menuProvider, ILoggerFactory loggerFactory, IEntityMemoryStore memoryStore)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _menuProvider = menuProvider;
            _memoryStore = memoryStore;
        }

        public async Task<IViewComponentResult> InvokeAsync(Tidings tidings)
        {
            try
            {
                var set = _memoryStore.NavigationItems.Where(x =>
                    x.GroupCode.ToUpper() == tidings[Tidings.DefinedKeys.Group].ToUpper()).ToArray();
                var tree = set.Trees();
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
                new Tiding(Tidings.DefinedKeys.AssemblyQualifiedName, typeof(MenuViewComponent).AssemblyQualifiedName),
                new Tiding(Tidings.DefinedKeys.Group, ""),
                new Tiding(Tidings.DefinedKeys.View, "")
            };
            return result;
        }
    }
}
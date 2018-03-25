using Borg.Cms.Basic.Lib.Features.Navigation.Contracts;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Conventions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Borg.Cms.Basic.Presentation.ActionFilters;
using Borg.Cms.Basic.Presentation.Services.Contracts;
using Borg.MVC.BuildingBlocks;

namespace Borg.Cms.Basic.Presentation.Areas.Presentation.Controllers
{
    [ControllerTheme("Bootstrap3")]
    public class MenusController : PresentationController
    {
        private readonly IComponentPageDescriptorService<int> _componentPageDescriptors;
        private readonly IMenuProvider _menuProvider;
        private readonly IEntityMemoryStore _memoryStore;

        public MenusController(ILoggerFactory loggerFactory, IMediator dispatcher, IComponentPageDescriptorService<int> componentPageDescriptors, IMenuProvider menuProvider, IEntityMemoryStore memoryStore) : base(loggerFactory, dispatcher)
        {
            _componentPageDescriptors = componentPageDescriptors;
            _menuProvider = menuProvider;
            _memoryStore = memoryStore;
        }
        [PopulateComponentFromRouteActionFilter]
        public async Task<IActionResult> SiteRoot()
        {
            var rootmenu = "home";
            var key = ComponentKey(rootmenu);
            var descr = await _componentPageDescriptors.Get(key);
            if (descr?.PageContent == null) return BadRequest($"no menu for path {rootmenu} was found");
            if (descr.Device == null) return BadRequest($"no structure for path {rootmenu} was found");
            PageContent(descr.PageContent);
            PageDevice(descr);
            return View("Root");
        }

        [PopulateComponentFromRouteActionFilter]
        public IActionResult Root(string rootmenu)
        {
            return View();
        }
        [PopulateComponentFromRouteActionFilter]
        public  IActionResult Leaf(string parentmenu, string childmenu)
        {

            return View();
        }


        private int ComponentKey(string rootmenu)
        {

            var trees = _memoryStore.NavigationItems.AsEnumerable();
            return trees.First(x => x.Path.ToLower().Trim('/') == rootmenu.ToLower()).Id;
        }
        private int ComponentKey(string parentmenu, string childmenu)
        {

            var trees = _memoryStore.NavigationItems.AsEnumerable();
            var candidates = trees.Where(x => x.Path.ToLower().Trim('/') == childmenu.ToLower());
            if (candidates.Count() == 1)
            {
                return candidates.Single().Id;
            }

            var parent = _memoryStore.NavigationItems.Single(x => candidates.Select(c => c.Taxonomy.ParentId).Contains(x.Id) && x.Path.Trim('/').ToLower() == parentmenu.ToLower());
            return candidates.Single(x => x.Taxonomy.ParentId == parent.Id).Id;
        }
    }
}
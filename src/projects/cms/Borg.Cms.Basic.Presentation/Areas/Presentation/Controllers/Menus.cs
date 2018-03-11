using Borg.Cms.Basic.Lib.Features.Navigation.Contracts;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Conventions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Presentation.Areas.Presentation.Controllers
{
    [ControllerTheme("Bootstrap3")]
    public class MenusController : PresentationController
    {
        private readonly IComponentPageDescriptorService<int> _componentPageDescriptors;
        private readonly IMenuProvider _menuProvider;

        public MenusController(ILoggerFactory loggerFactory, IMediator dispatcher, IComponentPageDescriptorService<int> componentPageDescriptors, IMenuProvider menuProvider) : base(loggerFactory, dispatcher)
        {
            _componentPageDescriptors = componentPageDescriptors;
            _menuProvider = menuProvider;
        }

        public async Task<IActionResult> SiteRoot()
        {
            var rootmenu = "home";
            var trees = await _menuProvider.Tree("UTL");
            var mnu = trees.Trees.AsEnumerable().FirstOrDefault(x => x.HumanKey.ToLower() == rootmenu.ToLower());
            var descr = await _componentPageDescriptors.Get(int.Parse(mnu.Key));
            if (descr?.PageContent == null) return BadRequest($"no menu for path {rootmenu} was found");
            if (descr.Device == null) return BadRequest($"no structure for path {rootmenu} was found");
            PageContent(descr.PageContent);
            PageDevice(descr);
            return View("Root");
        }

        [Route("{rootmenu}")]
        public async Task<IActionResult> Root(string rootmenu)
        {
            var trees = await _menuProvider.Tree("BSC");
            var mnu = trees.Trees.AsEnumerable().FirstOrDefault(x => x.HumanKey.ToLower() == rootmenu.ToLower());
            var descr = await _componentPageDescriptors.Get(int.Parse(mnu.Key));
            if (descr?.PageContent == null) return BadRequest($"no menu for path {rootmenu} was found");
            if (descr.Device == null) return BadRequest($"no structure for path {rootmenu} was found");
            PageContent(descr.PageContent);
            PageDevice(descr);
            return View();
        }

        public async Task<IActionResult> Leaf(string parentmenu, string childmenu)
        {
            var trees = await _menuProvider.Tree("BSC");
            var table = trees.Trees.Flatten();
            var mnu = table.Single(x => x.HumanKey.ToLower().Contains($"{parentmenu}/{childmenu}".ToLower()) && x.Depth > 1);
            var descr = await _componentPageDescriptors.Get(int.Parse(mnu.Key));
            if (descr?.PageContent == null) return BadRequest($"no menu for path {parentmenu}/{childmenu} was found");
            if (descr.Device == null) return BadRequest($"no structure for path {parentmenu}/{childmenu} was found");
            PageContent(descr.PageContent);
            PageDevice(descr);
            return View();
        }
    }
}
using Borg.Cms.Basic.Lib.Features.CMS.Queries;
using Borg.Cms.Basic.Lib.Features.Navigation.ViewModels;
using Borg.MVC.BuildingBlocks.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Navigation.ViewComponents
{
    public class MenuGroupsViewComponent : ViewComponent
    {
        private readonly IMediator _dispatcher;
        private readonly IPageOrchestrator<IPageContent, IDevice> _orchestrator;

        public MenuGroupsViewComponent(IMediator dispatcher, IPageOrchestrator<IPageContent, IDevice> orchestrator)
        {
            _dispatcher = dispatcher;
            _orchestrator = orchestrator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new MenusDashboardViewModel
            {
                Groups = (await _dispatcher.Send(new MenuGroupsRequest())).Payload.ToArray(),
            };
            return View(model);
        }
    }
}
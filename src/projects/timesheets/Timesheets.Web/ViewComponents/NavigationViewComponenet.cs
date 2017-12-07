using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Timesheets.Web.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            var result = View() as IViewComponentResult;
            return Task.FromResult(result);
        }
    }

    public class SidebarMenuViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            var result = View() as IViewComponentResult;
            return Task.FromResult(result);
        }
    }

    public class SidebarSearchFormViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            var result = View() as IViewComponentResult;
            return Task.FromResult(result);
        }
    }

    public class MainHeaderViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            var result = View() as IViewComponentResult;
            return Task.FromResult(result);
        }
    }

    public class MainHeaderLogoViewComponent : ViewComponent
    {
        private readonly IHostingEnvironment _env;
        public MainHeaderLogoViewComponent(IHostingEnvironment env)
        {
            _env = env;

        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            var result = View(new MainHeaderLogoViewModel()) as IViewComponentResult;
            return Task.FromResult(result);
        }
    }

    public class MainHeaderLogoViewModel
    {
        public MainHeaderLogoViewModel()
        {
            Title = "Timesheets";
            Logo =  "https://d3kdj0p3ajn4xa.cloudfront.net/assets/resources/timesheets/timesheets-922dedd704c0ffddeacf702971ce82bb4c1c07b65d63534ed7790bb621fa5a10.png";
        }

        public string Logo { get; }
        public string Title { get; }
    }

    public class MainFooterViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            var result = View() as IViewComponentResult;
            return Task.FromResult(result);
        }
    }

    public class HiddenSidebarViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            var result = View() as IViewComponentResult;
            return Task.FromResult(result);
        }
    }
}
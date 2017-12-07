using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Borg.MVC.BuildingBlocks
{
    public class DeviceLayoutFilter : ActionFilterAttribute
    {
        private readonly IPageOrchestrator<IPageContent, IDevice> _orchestrator;
        public DeviceLayoutFilter(IPageOrchestrator<IPageContent, IDevice> orchestrator, string layout = ""):base()
        {
            _orchestrator = orchestrator;
            Layout = layout;
        }
        public string Layout { get; } = string.Empty;

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
            _orchestrator.TryContextualize(context.Controller as Controller);
            _orchestrator.Device.Layout = Layout;
        }
    }
}

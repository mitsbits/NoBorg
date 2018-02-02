﻿using Borg.Infra;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Borg.MVC.BuildingBlocks
{
    public class DeviceLayoutFilter : ActionFilterAttribute
    {
        private readonly IPageOrchestrator<IPageContent, IDevice> _orchestrator;

        public DeviceLayoutFilter(IPageOrchestrator<IPageContent, IDevice> orchestrator, string layout = "") : base()
        {
            Preconditions.NotNull(orchestrator, nameof(orchestrator));
            _orchestrator = orchestrator;
            Layout = layout;
        }

        private string Layout { get; }

        //public override void OnResultExecuting(ResultExecutingContext context)
        //{
        //    base.OnResultExecuting(context);
        //    _orchestrator.TryContextualize(context.Controller as Controller);
        //    _orchestrator.Device.Layout = Layout;
        //}

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _orchestrator.TryContextualize(context.Controller as Controller);
            _orchestrator.Device.Layout = Layout;
        }
    }
}
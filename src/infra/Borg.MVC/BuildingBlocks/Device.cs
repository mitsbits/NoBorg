using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Borg.MVC.Services.Breadcrumbs;

namespace Borg.MVC.BuildingBlocks
{
    public class Device : IDevice, ICanContextualize, ICanContextualizeFromController,
        ICanContextualizeFromView
    {
        private const string ControllerKey = "controller";
        private const string ActionKey = "action";
        private const string AreaKey = "area";
        public string FriendlyName { get; set; }

        public string Path { get; set; }
        public string QueryString { get; set; }
        public string Domain { get; set; }
        public string Area { get; protected set; }
        public string Controller { get; protected set; }
        public string Action { get; protected set; }
        public string Layout { get; set; } = string.Empty;

        public Tidings Scripts { get; private set; } = new Tidings();
        public Breadcrumbs Breadcrumbs { get; } = new Breadcrumbs();
        public bool ContextAcquired => _populated;

        public void Contextualize(ViewContext context)
        {
            Populate(context);
        }

        public void Contextualize(Controller controller)
        {
            Populate(controller);
        }

        private bool _populated;

        private void Populate(ViewContext context)
        {
            if (ContextAcquired) return;

            PopulateInternal(context.ActionDescriptor, context.HttpContext, context.ViewBag);

            _populated = true;
        }

        private void Populate(Controller context)
        {
            if (ContextAcquired) return;

            PopulateInternal(context.ControllerContext.ActionDescriptor, context.HttpContext, context.ViewBag);

            _populated = true;
        }

        private void PopulateInternal(ActionDescriptor actionDescriptor, HttpContext httpContent, dynamic viewBag)
        {
            Controller = actionDescriptor.RouteValues.ContainsKey(ControllerKey) ? actionDescriptor.RouteValues[ControllerKey] : string.Empty;
            Area = actionDescriptor.RouteValues.ContainsKey(AreaKey) ? actionDescriptor.RouteValues[AreaKey] : string.Empty;
            Action = actionDescriptor.RouteValues.ContainsKey(ActionKey) ? actionDescriptor.RouteValues[ActionKey] : string.Empty;

            Path = httpContent.Request.Path;
            QueryString = httpContent.Request.QueryString.HasValue
                ? httpContent.Request.QueryString.Value
                : string.Empty;

            var dispUrl = httpContent.Request.GetDisplayUrl();
            var indx = Path.Equals(@"/") ? 0 : dispUrl.IndexOf(Path, StringComparison.OrdinalIgnoreCase);
            Domain = (indx <= 0 ? dispUrl : dispUrl.Substring(0, indx)).TrimEnd('/') + "/";

            if (viewBag.Scripts is Tidings tidings)
            {
                Scripts.AppendAndUpdate(tidings);
            }
        }
    }
}
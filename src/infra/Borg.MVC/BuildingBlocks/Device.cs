using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Services.Breadcrumbs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.MVC.BuildingBlocks
{
    public class Device : IDevice, ICanContextualize, ICanContextualizeFromController, ICanContextualizeFromView
    {
        private const string ControllerKey = "controller";
        private const string ActionKey = "action";
        private const string AreaKey = "area";

        private readonly string[] _definedRouteKeys = { ControllerKey, ActionKey, AreaKey };

        public string FriendlyName { get; set; }

        public string Path { get; set; }
        public string QueryString { get; set; }
        public string Domain { get; set; }
        public string Area { get; protected set; }
        public string Controller { get; protected set; }
        public string Action { get; protected set; }
        public IDictionary<string, string> RouteValues { get; } = new Dictionary<string, string>();
        public string Layout { get; set; } = string.Empty;
        public ICollection<Section> Sections { get; set; } = new HashSet<Section>();
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;
        public void SectionsClear()
        {
            Sections.Clear();
        }

        public void SectionAdd(ISection section)
        {
            Sections.Add(section as Section);
        }

        ICollection<ISection> IHaveSections.Sections => Sections.Cast<ISection>().ToList();

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

            PopulateInternal(context.ActionDescriptor, context.HttpContext, context.ViewBag, context.RouteData);

            _populated = true;
        }

        private void Populate(Controller context)
        {
            if (ContextAcquired) return;

            PopulateInternal(context.ControllerContext.ActionDescriptor, context.HttpContext, context.ViewBag, context.RouteData);

            _populated = true;
        }

        private void PopulateInternal(ActionDescriptor actionDescriptor, HttpContext httpContent, dynamic viewBag, RouteData routeData)
        {
            Controller = actionDescriptor.RouteValues.ContainsKey(ControllerKey) ? actionDescriptor.RouteValues[ControllerKey] : string.Empty;
            Area = actionDescriptor.RouteValues.ContainsKey(AreaKey) ? actionDescriptor.RouteValues[AreaKey] : string.Empty;
            Action = actionDescriptor.RouteValues.ContainsKey(ActionKey) ? actionDescriptor.RouteValues[ActionKey] : string.Empty;

            foreach (var item in routeData.Values)
            {
                if (!_definedRouteKeys.Contains(item.Key.ToLower()))
                {
                    RouteValues.Add(item.Key.ToLower(), item.Value.ToString());
                }
            }

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
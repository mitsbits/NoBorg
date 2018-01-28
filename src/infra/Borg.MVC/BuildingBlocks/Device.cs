using Borg.Infra;
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
using System.Collections.Specialized;
using System.Linq;
using Borg.CMS.BuildingBlocks;
using Borg.CMS.BuildingBlocks.Contracts;

namespace Borg.MVC.BuildingBlocks
{
    public class Device : IDevice, ICanContextualizeFromController, ICanContextualizeFromView
    {
        private const string ControllerKey = "controller";
        private const string ActionKey = "action";
        private const string AreaKey = "area";

        private readonly string[] _definedRouteKeys = { ControllerKey, ActionKey, AreaKey };

        #region IHaveAFriendlyName

        public string FriendlyName { get; set; }

        #endregion IHaveAFriendlyName

        #region IHaveAUrl

        public string Path { get; set; }
        public string QueryString { get; set; }
        public string Domain { get; set; }

        #endregion IHaveAUrl

        #region IHaveAController

        public string Area { get; protected set; }
        public string Controller { get; protected set; }
        public string Action { get; protected set; }
        public IDictionary<string, string[]> RouteValues { get; } = new Dictionary<string, string[]>();

        #endregion IHaveAController

        #region ICanRenderParentViewElements

        public Tidings Scripts { get; private set; } = new Tidings();
        public Breadcrumbs Breadcrumbs { get; } = new Breadcrumbs();

        #endregion ICanRenderParentViewElements

        #region IDeviceStructureInfo

        #region IHaveSections

        public ICollection<Section> Sections { get; set; } = new HashSet<Section>();

        public void SectionsClear()
        {
            Sections.Clear();
        }

        public void SectionAdd(ISection section)
        {
            Sections.Add(section as Section);
        }

        ICollection<ISection> IHaveSections.Sections => Sections.Cast<ISection>().ToList();
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;

        #endregion IHaveSections

        #region IHaveALayout

        public string Layout { get; set; } = string.Empty;

        #endregion IHaveALayout

        #endregion IDeviceStructureInfo

        #region ICanContextualize

        private bool _populated;
        public bool ContextAcquired => _populated;

        #endregion ICanContextualize

        #region ICanContextualizeFromView

        public void Contextualize(ViewContext context)
        {
            Preconditions.NotNull(context, nameof(context));
            Populate(context);
        }

        #endregion ICanContextualizeFromView

        #region ICanContextualizeFromController

        public void Contextualize(Controller controller)
        {
            Preconditions.NotNull(controller, nameof(controller));
            Populate(controller);
        }

        #endregion ICanContextualizeFromController

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
            Preconditions.NotNull(actionDescriptor, nameof(actionDescriptor));
            Preconditions.NotNull(httpContent, nameof(httpContent));
            Preconditions.NotNull(viewBag, nameof(viewBag));
            Preconditions.NotNull(routeData, nameof(routeData));

            Controller = actionDescriptor.RouteValues.ContainsKey(ControllerKey) ? actionDescriptor.RouteValues[ControllerKey] : string.Empty;
            Area = actionDescriptor.RouteValues.ContainsKey(AreaKey) ? actionDescriptor.RouteValues[AreaKey] : string.Empty;
            Action = actionDescriptor.RouteValues.ContainsKey(ActionKey) ? actionDescriptor.RouteValues[ActionKey] : string.Empty;

            foreach (var item in routeData.Values)
            {
                if (!_definedRouteKeys.Contains(item.Key.ToLower()))
                {
                    RouteValues.Add(item.Key.ToLower(), new[] { item.Value.ToString() });
                }
            }
            foreach (var q in httpContent.Request.Query)
            {
                if (!RouteValues.ContainsKey(q.Key)) RouteValues.Add(q.Key, q.Value.ToArray());
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
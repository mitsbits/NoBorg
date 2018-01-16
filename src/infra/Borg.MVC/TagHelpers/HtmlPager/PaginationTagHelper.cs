using System;
using Borg.Infra;
using Borg.Infra.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Borg.MVC.TagHelpers.HtmlPager
{
    [HtmlTargetElement("pagination")]
    public class PaginationTagHelper : TagHelper
    {
        private const string ControllerKey = "controller";
        private const string ActionKey = "action";
        private const string AreaKey = "area";

        private readonly IPaginationSettingsProvider _provider;

        public PaginationTagHelper(IPaginationSettingsProvider provider = null)

        {
            _provider = provider;
        }

        [HtmlAttributeName("model")]
        public IPagedResult Model { get; set; }

        [HtmlAttributeName("settings")]
        public Pagination.PaginationInfo Settings { get; set; } = new Pagination.PaginationInfo();

        [HtmlAttributeName("display-style")]
        public string DisplayStyle { get; set; } = string.Empty;

        [HtmlAttributeName("query")]
        public QueryString Query { get; set; } = new QueryString(null);

        [HtmlAttributeName("url-generator")]
        public Func<int, string> GeneratePageUrl { get; set; } = null;

        [HtmlAttributeName("page-variable")]
        public string PageVariable { get; set; } = string.Empty;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (_provider != null)
            {
                    Settings = GetSettings();
            }
            if (!Query.HasValue)
            {
                Query = ViewContext.HttpContext.Request.QueryString;
            }
            if (GeneratePageUrl == null)
            {
                var descriptor = ViewContext.ActionDescriptor;
                var urlHelper = new UrlHelper(ViewContext);
                GeneratePageUrl = i => UrlFromViewContext(urlHelper, descriptor, i);
            }
            var existsingClass = context.AllAttributes.ContainsName("class")
                ? context.AllAttributes["class"].Value.ToString()
                    : string.Empty;
            if (Model == null) throw new ArgumentNullException(nameof(Model));
            var content = Pagination.GetHtmlPager(Model, GeneratePageUrl, Query.ToDictionary(), Settings, null);
            var trimstart = content.IndexOf('>') + 1;
            var trimend = content.Length - content.LastIndexOf('<');
            var trimmed = content.Substring(trimstart, content.Length - trimend - trimstart);
            output.Content.Clear();
            output.TagName = Settings.OutputTagElement;

            //output.Content.AppendHtml(trimmed);
            output.Attributes.SetAttribute("class", $"{Settings.ElementClass} {existsingClass}");
            output.Content.SetHtmlContent(trimmed);
            var s = output.GetString();


        }

        private string UrlFromViewContext(UrlHelper urlHelper, ActionDescriptor descriptor, int i)
        {
            var raw = urlHelper.Action(new UrlActionContext()
            {
                Action = descriptor.RouteValues.ContainsKey(ActionKey)
                    ? descriptor.RouteValues[ActionKey]
                    : string.Empty,
                Controller = descriptor.RouteValues.ContainsKey(ControllerKey)
                    ? descriptor.RouteValues[ControllerKey]
                    : string.Empty,
                Values = new
                {
                    Area = descriptor.RouteValues.ContainsKey(AreaKey) ? descriptor.RouteValues[AreaKey] : string.Empty,
                }
            });
            return $"{raw}?{Settings.PageVariableName}={i}";
        }

        private Pagination.PaginationInfo GetSettings()
        {
            Pagination.PaginationInfo resut;
            switch (DisplayStyle)
            {
                case Pagination.DisplayFormat.DefaultPager:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToNextPage = true,
                        DisplayLinkToPreviousPage = true,
                        DisplayPageCountAndCurrentLocation = true,
                        DisplayLinkToIndividualPages = true,
                        MaximumPageNumbersToDisplay = 10,
                        DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                        PagerInChunks = false,
                    };
                    break;
                case Pagination.DisplayFormat.MinimalWithItemCountText:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToNextPage = true,
                        DisplayLinkToPreviousPage = true,
                        DisplayItemSliceAndTotal = true,
                        PagerInChunks = false,
                    };
                    break;
                case Pagination.DisplayFormat.DefaultPlusFirstAndLast:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToFirstPage = true,
                        DisplayLinkToLastPage = true,
                        DisplayPageCountAndCurrentLocation = true,
                        PagerInChunks = false,
                    };
                    break;
                case Pagination.DisplayFormat.Minimal:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToNextPage = true,
                        DisplayLinkToPreviousPage = true,
                        PagerInChunks = false,
                    };
                    break;
                case Pagination.DisplayFormat.MinimalWithPageCountText:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToNextPage = true,
                        DisplayLinkToPreviousPage = true,
                        DisplayPageCountAndCurrentLocation = true,
                        PagerInChunks = false,
                    };
                    break;
                case Pagination.DisplayFormat.MinimalWithPages:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToFirstPage = false,
                        DisplayLinkToLastPage = false,
                        DisplayLinkToPreviousPage = true,
                        DisplayLinkToNextPage = true,
                        DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                        DisplayPageCountAndCurrentLocation = false,
                        PagerInChunks = false,
                        DisplayLinkToIndividualPages = true,
                        MaximumPageNumbersToDisplay = 10,
                    };
                    break;
                case Pagination.DisplayFormat.PageNumbersOnly:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToFirstPage = false,
                        DisplayLinkToLastPage = false,
                        DisplayLinkToPreviousPage = false,
                        DisplayLinkToNextPage = false,
                        DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                        DisplayLinkToIndividualPages = true,
                        PagerInChunks = false,
                    };
                    break;
                case Pagination.DisplayFormat.PagerInChucks:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToFirstPage = false,
                        DisplayLinkToLastPage = false,
                        DisplayLinkToPreviousPage = true,
                        DisplayLinkToNextPage = true,
                        DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                        PagerInChunks = true,
                    };
                    break;
                default:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToNextPage = true,
                        DisplayLinkToPreviousPage = true,
                        DisplayPageCountAndCurrentLocation = true,
                        DisplayLinkToIndividualPages = true,
                        MaximumPageNumbersToDisplay = 10,
                        DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                        PagerInChunks = false,
                    };
                    break;
            }

            if (!string.IsNullOrWhiteSpace(PageVariable)) resut.PageVariableName = PageVariable;
            return resut;
        }
    }
}

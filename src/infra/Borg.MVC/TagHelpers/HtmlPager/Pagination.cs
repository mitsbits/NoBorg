using Borg.Infra.Collections;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using Borg.Infra;

namespace Borg.MVC.TagHelpers.HtmlPager
{
    public static partial class Pagination
    {
        public static class DisplayFormat
        {
            public const string DefaultPlusFirstAndLast = "DefaultPlusFirstAndLast";
            public const string Minimal = "Minimal";
            public const string MinimalWithPageCountText = "MinimalWithPageCountText";
            public const string MinimalWithItemCountText = "MinimalWithItemCountText";
            public const string MinimalWithPages = "MinimalWithPages";
            public const string DefaultPager = "DefaultPager";
            public const string PageNumbersOnly = "PageNumbersOnly";
            public const string PagerInChucks = "PagerInChucks";
        }

        public static HtmlString HtmlPager<T>(
            this IHtmlHelper helper,
            IPagedResult<T> metaData,
            Func<int, string> generatePageUrl,
            QueryString query,
            PaginationInfo settings = null,
            object htmlAttributes = null)
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData), "A navigation collection is mandatory.");
            if (!metaData.Any()) return HtmlString.Empty;
            if (settings == null) settings = new PaginationInfo();
            return
                new HtmlString(GetHtmlPager(metaData, generatePageUrl, query.ToDictionary(), settings, htmlAttributes));
        }

        public static HtmlString HtmlPager<T>(
            this IHtmlHelper helper,
            IPagedResult<T> metaData,
            Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues = null,
            PaginationInfo settings = null,
            object htmlAttributes = null)
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData), "A navigation collection is mandatory.");
            if (!metaData.Any()) return HtmlString.Empty;
            if (settings == null) settings = new PaginationInfo();
            return new HtmlString(GetHtmlPager(metaData, generatePageUrl, routedValues, settings, htmlAttributes));
        }

        internal static string GetHtmlPager(
            IPagedResult metaData,
            Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues,
            PaginationInfo settings,
            object htmlAttributes)
        {
            var listItemLinks = new List<TagBuilder>();

            //first

            if (settings.DisplayLinkToFirstPage)
                listItemLinks.Add(First(metaData, generatePageUrl, routedValues, settings));

            if (settings.DisplayLinkToPreviousPage)
                listItemLinks.Add(Previous(metaData, generatePageUrl, routedValues, settings));

            //text
            if (settings.DisplayPageCountAndCurrentLocation)
                listItemLinks.Add(PageCountAndLocationText(metaData, settings));

            //text
            if (settings.DisplayItemSliceAndTotal)
                listItemLinks.Add(ItemSliceAndTotalText(metaData, settings));

            //page
            if (!settings.PagerInChunks)
            {
                if (settings.DisplayLinkToIndividualPages)
                {
                    //calculate start and end of range of page numbers
                    var start = 1;
                    var end = metaData.TotalPages;
                    if (settings.MaximumPageNumbersToDisplay.HasValue &&
                        metaData.TotalPages > settings.MaximumPageNumbersToDisplay)
                    {
                        var maxPageNumbersToDisplay = settings.MaximumPageNumbersToDisplay.Value;
                        start = metaData.Page - maxPageNumbersToDisplay / 2;
                        if (start < 1)
                            start = 1;
                        end = maxPageNumbersToDisplay;
                        if ((start + end - 1) > metaData.TotalPages)
                            start = metaData.TotalPages - maxPageNumbersToDisplay + 1;
                    }

                    //if there are previous page numbers not displayed, show an ellipsis
                    if (settings.DisplayEllipsesWhenNotShowingAllPageNumbers && start > 1)
                        listItemLinks.Add(EllipsesPrevious(metaData, generatePageUrl, routedValues, settings));

                    foreach (var i in Enumerable.Range(start, end))
                    {
                        //show page number link
                        listItemLinks.Add(Page(i, metaData, generatePageUrl, routedValues, settings));
                    }

                    //if there are subsequent page numbers not displayed, show an ellipsis
                    if (settings.DisplayEllipsesWhenNotShowingAllPageNumbers && (start + end - 1) < metaData.TotalPages)
                        listItemLinks.Add(EllipsesNext(metaData, generatePageUrl, routedValues, settings));
                }
            }
            else //show page links in chunks
            {
                int current = metaData.Page;

                int chunckStart = current;

                if (current % settings.ChunkCount != 0)
                {
                    while (chunckStart % settings.ChunkCount != 0)
                    {
                        chunckStart -= 1;
                    }
                }
                else
                {
                    chunckStart = current - settings.ChunkCount;
                }
                foreach (var i in Enumerable.Range(chunckStart + 1, settings.ChunkCount))
                {
                    //show page number link
                    listItemLinks.Add(Page(i, metaData, generatePageUrl, routedValues, settings));
                }
            }

            //next
            if (settings.DisplayLinkToNextPage)
                listItemLinks.Add(Next(metaData, generatePageUrl, routedValues, settings));

            //last
            if (settings.DisplayLinkToLastPage)
                listItemLinks.Add(Last(metaData, generatePageUrl, routedValues, settings));

            //collapse all of the list items into one big string
            string listItemLinksString = null;

            listItemLinksString = listItemLinks.Aggregate(
                new StringBuilder(),
                (sb, listItem) => sb.Append(listItem.GetString()),
                sb => sb.ToString()
            );

            var ul = new TagBuilder("ul");
            ul.InnerHtml.AppendHtml(listItemLinksString);

            ul.AddCssClass(settings.ElementClass);
            if (htmlAttributes != null) ul.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return ul.GetString();
        }

        private static string GetRoutedValues(IDictionary<string, string[]> routedValues, string pageVariable)
        {
            //string paramBuilder = string.Empty;

            StringBuilder paramBuilder = new StringBuilder(string.Empty);

            if (routedValues.Count > 0)
            {
                foreach (string key in routedValues.Keys)
                {
                    foreach (var val in routedValues[key])
                    {
                        if (!key.Equals(pageVariable, StringComparison.OrdinalIgnoreCase))
                            paramBuilder.Append($"&{key}={val}");
                    }
                }
            }

            return paramBuilder.ToString();
        }

        private static TagBuilder Next(IPagedResult metadata,
            Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues,
            PaginationInfo settings)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.ArrowClass + " next");
            var targetPageNumber = metadata.Page + 1;
            var next = new TagBuilder("a");
            next.InnerHtml.Append(settings.NextDisplay);
            if (metadata.HasNextPage)
            {
                next.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        settings.PageVariableName), true);
            }
            else
            {
                item.AddCssClass(settings.UnavailableClass);
                next.MergeAttribute("href",
                    string.Empty, true);
            }
            var htmlContentBuilder = item.InnerHtml.AppendHtml(next);
            return item;
        }

        private static TagBuilder Last(IPagedResult metadata,
            Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues,
            PaginationInfo settings)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.ArrowClass);
            var targetPageNumber = metadata.TotalPages;
            var last = new TagBuilder("a");
            last.InnerHtml.Append(settings.LastDisplay);
            if (metadata.Page == metadata.TotalPages)
            {
                item.AddCssClass(settings.UnavailableClass);
                last.MergeAttribute("href",
                    string.Empty, true);
            }
            else
            {
                last.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        settings.PageVariableName), true);
            }
            var htmlContentBuilder = item.InnerHtml.AppendHtml(last);
            return item;
        }

        private static TagBuilder PageCountAndLocationText(IPagedResult metadata, PaginationInfo settings)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.UnavailableClass);
            var text = new TagBuilder("a");

            text.InnerHtml.AppendHtml(string.Format(settings.PageCountAndLocationFormat, metadata.Page,
                metadata.TotalPages));
            text.MergeAttribute("href",
                string.Empty, true);
            var htmlContentBuilder = item.InnerHtml.AppendHtml(text);
            return item;
        }

        private static TagBuilder ItemSliceAndTotalText(IPagedResult metadata, PaginationInfo settings)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.UnavailableClass);

            int FirstItemOnPage = (metadata.Page - 1) * metadata.PageSize + 1;
            var numberOfLastItemOnPage = FirstItemOnPage + metadata.PageSize - 1;
            int LastItemOnPage = numberOfLastItemOnPage > metadata.TotalRecords
                ? metadata.TotalRecords
                : numberOfLastItemOnPage;

            var text = new TagBuilder("a");
            text.InnerHtml.AppendHtml(string.Format(settings.ItemSliceAndTotalFormat, FirstItemOnPage, LastItemOnPage,
                metadata.TotalRecords));
            text.MergeAttribute("href",
                string.Empty, true);
            item.InnerHtml.AppendHtml(text);
            return item;
        }

        private static TagBuilder EllipsesPrevious(IPagedResult metaData, Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues, PaginationInfo settings)
        {
            var targetPageNumber = metaData.Page - (settings.MaximumPageNumbersToDisplay ?? 10);
            if (targetPageNumber < 1) targetPageNumber = 1;

            var item = new TagBuilder(settings.OutputItemTagElement);
            var a = new TagBuilder("a");
            a.InnerHtml.Append(settings.PaginationInfoStyle.Ellipses);
            if (targetPageNumber == metaData.Page)
            {
                item.AddCssClass(settings.UnavailableClass);
                a.MergeAttribute("href",
                    string.Empty, true);
            }
            else
            {
                a.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        settings.PageVariableName), true);
            }
            var htmlContentBuilder = item.InnerHtml.AppendHtml(a);
            return item;
        }

        private static TagBuilder EllipsesNext(IPagedResult metaData, Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues, PaginationInfo settings)
        {
            var targetPageNumber = metaData.Page + (settings.MaximumPageNumbersToDisplay ?? 10);
            if (targetPageNumber > metaData.TotalPages) targetPageNumber = metaData.TotalPages;

            var item = new TagBuilder(settings.OutputItemTagElement);
            var a = new TagBuilder("a");
            a.InnerHtml.Append(settings.PaginationInfoStyle.Ellipses);
            if (targetPageNumber == metaData.Page)
            {
                item.AddCssClass(settings.UnavailableClass);
                a.MergeAttribute("href",
                    string.Empty, true);
            }
            else
            {
                a.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        settings.PageVariableName), true);
            }
            var htmlContentBuilder = item.InnerHtml.AppendHtml(a);
            return item;
        }

        private static TagBuilder Page(int i, IPagedResult metaData, Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues, PaginationInfo settings)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);

            var targetPageNumber = i;
            var page = new TagBuilder("a");
            page.InnerHtml.AppendHtml(string.Format(settings.PageDisplayFormat, i));

            if (metaData.Page == i)
            {
                item.AddCssClass(settings.CurrentClass);
                item.AddCssClass(settings.UnavailableClass);
                page.MergeAttribute("href",
                    string.Empty, true);
            }
            else
            {
                page.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        settings.PageVariableName), true);
            }

            var htmlContentBuilder = item.InnerHtml.AppendHtml(page);
            return item;
        }

        private static TagBuilder Previous(IPagedResult metaData, Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues, PaginationInfo settings)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.ArrowClass + " previous");
            var targetPageNumber = metaData.Page - 1;
            var previous = new TagBuilder("a");
            previous.InnerHtml.Append(settings.PreviousDisplay);
            if (targetPageNumber < 1)
            {
                item.AddCssClass(settings.UnavailableClass);
                previous.MergeAttribute("href",
                    string.Empty, true);
            }
            else
            {
                previous.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        settings.PageVariableName), true);
            }

            var htmlContentBuilder = item.InnerHtml.AppendHtml(previous);
            return item;
        }

        private static TagBuilder First(IPagedResult metaData, Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues, PaginationInfo settings)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);

            const int targetPageNumber = 1;
            var first = new TagBuilder("a");
            first.InnerHtml.Append(settings.FirstDisplay);
            if (metaData.Page == targetPageNumber)
            {
                item.AddCssClass(settings.UnavailableClass);
                first.MergeAttribute("href",
                    string.Empty, true);
            }
            else
            {
                first.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        settings.PageVariableName), true);
            }

            var htmlContentBuilder = item.InnerHtml.AppendHtml(first);
            return item;
        }

        public class PaginationInfo
        {
            public PaginationInfo()
            {
            }

            public PaginationInfo(IPaginationSettingsProvider provider) : this()
            {
                if (provider != null)
                {
                    var instance = provider.Style;
                    _paginationInfoStyle.ArrowClass = instance.ArrowClass;
                    _paginationInfoStyle.CurrentClass = instance.CurrentClass;
                    _paginationInfoStyle.ElementClass = instance.ElementClass;
                    _paginationInfoStyle.Ellipses = instance.Ellipses;
                    _paginationInfoStyle.FirstDisplay = instance.FirstDisplay;
                    _paginationInfoStyle.ItemSliceAndTotalFormat = instance.ItemSliceAndTotalFormat;
                    _paginationInfoStyle.LastDisplay = instance.LastDisplay;
                    _paginationInfoStyle.NextDisplay = instance.NextDisplay;
                    _paginationInfoStyle.PageCountAndLocationFormat = instance.PageCountAndLocationFormat;
                    _paginationInfoStyle.PageDisplayFormat = instance.PageDisplayFormat;
                    _paginationInfoStyle.PageVariableName = instance.PageVariableName;
                    _paginationInfoStyle.PreviousDisplay = instance.PreviousDisplay;
                    _paginationInfoStyle.UnavailableClass = instance.UnavailableClass;
                    _paginationInfoStyle.OutputTagElement = instance.OutputTagElement;
                    _paginationInfoStyle.OutputItemTagElement = instance.OutputItemTagElement;
                }
            }

            [DefaultValue(false)]
            public bool PagerInChunks { get; set; } = false;

            public int ChunkCount { get; set; } = 10;

            private readonly PaginationInfoStyle _paginationInfoStyle = new PaginationInfoStyle();

            public PaginationInfoStyle PaginationInfoStyle
            {
                get { return _paginationInfoStyle; }
            }

            [DefaultValue("{0} to {1} of {2}")]
            public virtual string ItemSliceAndTotalFormat
            {
                set { _paginationInfoStyle.ItemSliceAndTotalFormat = value; }
                get { return _paginationInfoStyle.ItemSliceAndTotalFormat; }
            }

            [DefaultValue("{0} of {1}")]
            public virtual string PageCountAndLocationFormat
            {
                set { _paginationInfoStyle.PageCountAndLocationFormat = value; }
                get { return _paginationInfoStyle.PageCountAndLocationFormat; }
            }

            [DefaultValue(">")]
            public virtual string NextDisplay
            {
                set { _paginationInfoStyle.NextDisplay = value; }
                get { return _paginationInfoStyle.NextDisplay; }
            }

            [DefaultValue(">>")]
            public virtual string LastDisplay
            {
                set { _paginationInfoStyle.LastDisplay = value; }
                get { return _paginationInfoStyle.LastDisplay; }
            }

            [DefaultValue("<")]
            public virtual string PreviousDisplay
            {
                set { _paginationInfoStyle.PreviousDisplay = value; }
                get { return _paginationInfoStyle.PreviousDisplay; }
            }

            [DefaultValue("<<")]
            public virtual string FirstDisplay
            {
                set { _paginationInfoStyle.FirstDisplay = value; }
                get { return _paginationInfoStyle.FirstDisplay; }
            }

            [DefaultValue("{0}")]
            public virtual string PageDisplayFormat
            {
                set { _paginationInfoStyle.PageDisplayFormat = value; }
                get { return _paginationInfoStyle.PageDisplayFormat; }
            }

            [DefaultValue("page")]
            public virtual string PageVariableName
            {
                set { _paginationInfoStyle.PageVariableName = value; }
                get { return _paginationInfoStyle.PageVariableName; }
            }

            [DefaultValue("pagination")]
            public virtual string ElementClass
            {
                set { _paginationInfoStyle.ElementClass = value; }
                get { return _paginationInfoStyle.ElementClass; }
            }

            [DefaultValue("current")]
            public virtual string CurrentClass
            {
                set { _paginationInfoStyle.CurrentClass = value; }
                get { return _paginationInfoStyle.CurrentClass; }
            }

            [DefaultValue("unavailable")]
            public virtual string UnavailableClass
            {
                set { _paginationInfoStyle.UnavailableClass = value; }
                get { return _paginationInfoStyle.UnavailableClass; }
            }

            [DefaultValue("arrow")]
            public virtual string ArrowClass
            {
                set { _paginationInfoStyle.ArrowClass = value; }
                get { return _paginationInfoStyle.ArrowClass; }
            }

            [DefaultValue("ul")]
            public virtual string OutputTagElement
            {
                set { _paginationInfoStyle.OutputTagElement = value; }
                get { return _paginationInfoStyle.OutputTagElement; }
            }

            [DefaultValue("li")]
            public virtual string OutputItemTagElement
            {
                set { _paginationInfoStyle.OutputItemTagElement = value; }
                get { return _paginationInfoStyle.OutputItemTagElement; }
            }

            #region List Render options

            ///<summary>
            /// When true, includes a hyperlink to the first page of the list.
            ///</summary>
            public bool DisplayLinkToFirstPage { get; set; }

            ///<summary>
            /// When true, includes a hyperlink to the last page of the list.
            ///</summary>
            public bool DisplayLinkToLastPage { get; set; }

            ///<summary>
            /// When true, includes a hyperlink to the previous page of the list.
            ///</summary>
            public bool DisplayLinkToPreviousPage { get; set; }

            ///<summary>
            /// When true, includes a hyperlink to the next page of the list.
            ///</summary>
            public bool DisplayLinkToNextPage { get; set; }

            ///<summary>
            /// When true, includes hyperlinks for each page in the list.
            ///</summary>
            public bool DisplayLinkToIndividualPages { get; set; }

            ///<summary>
            /// When true, shows the current page number and the total number of pages in the list.
            ///</summary>
            ///<example>
            /// "Page 3 of 8."
            ///</example>
            public bool DisplayPageCountAndCurrentLocation { get; set; }

            ///<summary>
            /// When true, shows the one-based index of the first and last items on the page, and the total number of items in the list.
            ///</summary>
            ///<example>
            /// "Showing items 75 through 100 of 183."
            ///</example>
            public bool DisplayItemSliceAndTotal { get; set; }

            ///<summary>
            /// The maximum number of page numbers to display. Null displays all page numbers.
            ///</summary>
            public int? MaximumPageNumbersToDisplay { get; set; }

            ///<summary>
            /// If true, adds an ellipsis where not all page numbers are being displayed.
            ///</summary>
            ///<example>
            /// "1 2 3 4 5 ...",
            /// "... 6 7 8 9 10 ...",
            /// "... 11 12 13 14 15"
            ///</example>
            public bool DisplayEllipsesWhenNotShowingAllPageNumbers { get; set; }

            #endregion List Render options

            ///<summary>
            /// Also includes links to First and Last pages.
            ///</summary>
            public static PaginationInfo DefaultPlusFirstAndLast
            {
                get
                {
                    var result =
                        new PaginationInfo
                        {
                            DisplayLinkToFirstPage = true,
                            DisplayLinkToLastPage = true,
                            DisplayPageCountAndCurrentLocation = true,
                            PagerInChunks = false,
                        };
                    return result;
                }
            }

            ///<summary>
            /// Shows only the Previous and Next links.
            ///</summary>
            public static PaginationInfo Minimal
            {
                get
                {
                    var result =
                        new PaginationInfo
                        {
                            DisplayLinkToNextPage = true,
                            DisplayLinkToPreviousPage = true,
                            PagerInChunks = false,
                        };
                    return result;
                }
            }

            ///<summary>
            /// Shows Previous and Next links along with current page number and page count.
            ///</summary>
            public static PaginationInfo MinimalWithPageCountText
            {
                get
                {
                    var result =
                        new PaginationInfo
                        {
                            DisplayLinkToNextPage = true,
                            DisplayLinkToPreviousPage = true,
                            DisplayPageCountAndCurrentLocation = true,
                            PagerInChunks = false,
                        };
                    return result;
                }
            }

            ///<summary>
            ///	Shows Previous and Next links along with index of first and last items on page and total number of items across all pages.
            ///</summary>
            public static PaginationInfo MinimalWithItemCountText
            {
                get
                {
                    var result =
                        new PaginationInfo
                        {
                            DisplayLinkToNextPage = true,
                            DisplayLinkToPreviousPage = true,
                            DisplayItemSliceAndTotal = true,
                            PagerInChunks = false,
                        };

                    return result;
                }
            }

            public static PaginationInfo MinimalWithPages
            {
                get
                {
                    var result =
                        new PaginationInfo
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
                    return result;
                }
            }

            public static PaginationInfo DefaultPager
            {
                get
                {
                    var result =
                        new PaginationInfo
                        {
                            DisplayLinkToNextPage = true,
                            DisplayLinkToPreviousPage = true,
                            DisplayPageCountAndCurrentLocation = true,
                            DisplayLinkToIndividualPages = true,
                            MaximumPageNumbersToDisplay = 10,
                            DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                            PagerInChunks = false,
                        };
                    return result;
                }
            }

            ///<summary>
            ///	Shows only links to each individual page.
            ///</summary>
            public static PaginationInfo PageNumbersOnly
            {
                get
                {
                    var result =
                        new PaginationInfo
                        {
                            DisplayLinkToFirstPage = false,
                            DisplayLinkToLastPage = false,
                            DisplayLinkToPreviousPage = false,
                            DisplayLinkToNextPage = false,
                            DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                            PagerInChunks = false,
                        };
                    return result;
                }
            }

            public static PaginationInfo PagerInChucks
            {
                get
                {
                    var result =
                        new PaginationInfo
                        {
                            DisplayLinkToFirstPage = false,
                            DisplayLinkToLastPage = false,
                            DisplayLinkToPreviousPage = true,
                            DisplayLinkToNextPage = true,
                            DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                            PagerInChunks = true,
                        };
                    return result;
                }
            }
        }

        public static string GetString(this IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
using Borg.Infra;
using Borg.MVC.BuildingBlocks.Contracts;
using System;
using Borg.CMS.Components.Contracts;

namespace Borg.MVC.Extensions
{
    public static class HtmlMetaExtensions
    {
        public static bool IsHttpEquiv(this IHtmlMeta meta)
        {
            return !string.IsNullOrWhiteSpace(meta.HttpEquiv);
        }

        public static bool IsOpenGraph(this IHtmlMeta meta)
        {
            Preconditions.NotNull(meta, nameof(meta));
            return (!string.IsNullOrWhiteSpace(meta.Name)
                    && meta.Name.StartsWith("og:", StringComparison.CurrentCultureIgnoreCase));
        }

        public static bool IsTwitterCard(this IHtmlMeta meta)
        {
            Preconditions.NotNull(meta, nameof(meta));
            return (!string.IsNullOrWhiteSpace(meta.Name)
                    && meta.Name.StartsWith("twitter:", StringComparison.CurrentCultureIgnoreCase));
        }

        public static string TypeIdentifier(this IHtmlMeta meta)
        {
            Preconditions.NotNull(meta, nameof(meta));
            if (meta.IsOpenGraph()) return "OPENGRAPH";
            if (meta.IsTwitterCard()) return "TWITTERCARD";
            return meta.IsHttpEquiv() ? "HTTPEQUIV" : "BASIC";
        }
    }
}
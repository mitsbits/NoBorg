using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.MVC.TagHelpers.HtmlPager
{
    internal static class PaginationHelperExtensions
    {
        internal static IDictionary<string, string[]> ToDictionary(this QueryString query)
        {
            if (!query.HasValue) return new Dictionary<string, string[]>();
            var q = query.Value.TrimStart('?').Split('&');
            return q.Select(x => Tuple.Create<string, string>(x.Split('=')[0], (x.Split('=').Length > 1) ? x.Split('=')[1] : string.Empty))
                .GroupBy(t => t.Item1).Where(g => !string.IsNullOrWhiteSpace(g.Key))
                .ToDictionary(x => x.Key, x => x.Where(v => !string.IsNullOrWhiteSpace(v.Item2)).Select(v => v.Item2).ToArray());
        }
    }
}
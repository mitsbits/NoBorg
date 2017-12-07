using Borg.MVC.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace Borg.MVC.TagHelpers
{
    [HtmlTargetElement("concat-text")]
    public class ConcatenateStringsTagHelper : TagHelper
    {
        [HtmlAttributeName("data")]
        public IEnumerable<string> Data { get; set; }

        [HtmlAttributeName("seperator")]
        public string Seperator { get; set; } = ", ";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var text = string.Join(Seperator, Data);
            output.TagName = string.Empty;
            var tagHelperContent = output.Content.SetContent(text);
        }
    }
}

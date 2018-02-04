using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.TagHelpers
{
    [HtmlTargetElement("span", Attributes = "doc-published")]
    [PulgInTagHelper("Document Published State")]
    public sealed class DocumentPublishedStateTagHelper : TagHelper
    {
        public bool PublishedState { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Clear();
            var tag = new TagBuilder("span");
            tag.MergeAttribute("title", PublishedState ? "Published" : "Suspended");
            tag.AddCssClass(PublishedState
                ? "glyphicon glyphicon-ok-sign text-success"
                : "glyphicon glyphicon-remove-sign text-danger");
            output.Content.AppendHtml(tag.GetString());
            output.TagName = "";
        }
    }
}
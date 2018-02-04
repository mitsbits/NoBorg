using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.TagHelpers
{
    [HtmlTargetElement("span", Attributes = "doc-deleted")]
    [PulgInTagHelper("Document Deleted State")]
    public sealed class DocumentDeletedStateTagHelper : TagHelper
    {
        public bool DeletedState { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Clear();
            var tag = new TagBuilder("span");
            tag.MergeAttribute("title", DeletedState ? "Deleted" : "Active");
            tag.AddCssClass(DeletedState
                ? "glyphicon glyphicon-remove text-danger"
                : "glyphicon glyphicon-ok text-success");
            output.Content.AppendHtml(tag.GetString());
            output.TagName = "";
        }
    }
}
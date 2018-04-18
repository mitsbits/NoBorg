using Borg.MVC.Services.Editors;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.ViewModels
{
    public class ArticlePageMetadataViewModel
    {
        public int RecordId { get; set; }
        public JsonEditor HtmlMetas { get; set; }
    }
}
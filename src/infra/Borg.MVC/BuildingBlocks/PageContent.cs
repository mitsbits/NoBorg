using Borg.MVC.BuildingBlocks.Contracts;
using System.Collections.Generic;

namespace Borg.MVC.BuildingBlocks
{
    public class PageContent : IPageContent
    {
        public HtmlMetaSet Metas { get; } = new HtmlMetaSet();
       
        public string Title { get; set; }
        public string Subtitle { get; set; }

        public string[] Body { get; set; }

        public void SetTitle(string title)
        {
            Title = title.Trim();
        }
    }
}
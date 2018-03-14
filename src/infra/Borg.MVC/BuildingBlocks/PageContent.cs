using Borg.CMS.Components;
using Borg.MVC.BuildingBlocks.Contracts;

namespace Borg.MVC.BuildingBlocks
{
    public class PageContent : IPageContent
    {
        public int Id { get; set; }

        public void Delete()
        {
            throw new System.NotImplementedException();
        }

        public bool IsDeleted { get; }
        public bool IsPublished { get; private set; }

        public void Publish()
        {
            IsPublished = true;
        }

        public void Suspend()
        {
            IsPublished = false;
        }

        public string Title { get; set; }
        public string Slug { get; }
        public string RelativePath { get; }
        public string APrimaryImage { get; }
        public HtmlMetaSet Metas { get; } = new HtmlMetaSet();
        public TagSet Tags { get; } = new TagSet();
        public string MainContent { get; set; }
        public string PrimaryImageFileId { get; set; }
        public string Subtitle { get; set; }
        public string ComponentKey { get; set; }
    }
}
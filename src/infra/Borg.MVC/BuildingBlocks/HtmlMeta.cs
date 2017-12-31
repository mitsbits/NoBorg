using Borg.MVC.BuildingBlocks.Contracts;

namespace Borg.MVC.BuildingBlocks
{
    public class HtmlMeta : IHtmlMeta
    {
        public virtual string Content
        {
            get;
            set;
        }

        public virtual string HttpEquiv
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Scheme
        {
            get;
            set;
        }

        public virtual bool ShouldRender => !string.IsNullOrWhiteSpace(Content) && (!string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(HttpEquiv));
    }
}
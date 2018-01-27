using Borg.Infra.DDD.Contracts;

namespace Borg.Platform.EF.CMS
{
    public class ComponentState : IEntity<int>
    {
        public int Id { get; protected set; }
        public bool IsDeleted { get;  set; }
        public bool IsPublished { get;  set; }

        internal virtual TagState Tag { get; set; }
        internal virtual HtmlSnippetState HtmlSnippet { get; set; }
        internal virtual ArticleState Article { get; set; }
        internal virtual TaxonomyState Taxonomy { get; set; }
    }
}
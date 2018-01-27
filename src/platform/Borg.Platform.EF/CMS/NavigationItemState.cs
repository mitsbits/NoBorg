using Borg.CMS;
using Borg.Infra.DDD.Contracts;

namespace Borg.Platform.EF.CMS
{
    public class NavigationItemState : IEntity<int>
    {
        public int Id { get; set; }
        public string GroupCode { get; set; }
        public string Path { get; set; }
        public NavigationItemType NavigationItemType { get; set; }
        public virtual TaxonomyState Taxonomy { get; set; }
        public virtual ComponentState Component => Taxonomy?.Component;
        public virtual ArticleState Article => Taxonomy?.Article;
    }
}
using Borg.Infra.DDD.Contracts;

namespace Borg.Platform.EF.CMS
{
    public class TaxonomyState : IEntity<int>
    {
        public int Id { get; protected set; }
        public virtual ComponentState Component { get; set; }
        public int ParentId { get; set; } = 0;
        public int ArticleId { get; protected set; }
        public double Weight { get; set; }
        public virtual ArticleState Article { get; set; }
        internal virtual NavigationItemState NavigationItem { get; set; }
    }
}
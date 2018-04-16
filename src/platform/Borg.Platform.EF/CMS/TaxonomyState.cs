using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public class TaxonomyState : IEntity<int>
    {
        public int Id { get; set; }
        public virtual ComponentState Component { get; set; }
        public int ParentId { get; set; } = 0;
        public int ArticleId { get; protected set; }
        public double Weight { get; set; }
        public virtual ArticleState Article { get; set; }
        internal virtual NavigationItemState NavigationItem { get; set; }
        internal virtual CategoryState Category { get; set; }
    }

    public partial class ComponentState
    {
        internal TaxonomyState Taxonomy { get; set; }
    }

    public class TaxonomyStateMap : EntityMap<TaxonomyState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TaxonomyState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<TaxonomyState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<TaxonomyState>().Property(x => x.ParentId).IsRequired().HasDefaultValue(0);
            builder.Entity<TaxonomyState>().HasOne(x => x.Article).WithOne(a => a.Taxonomy);
            builder.Entity<TaxonomyState>().HasIndex(x => x.ArticleId).IsUnique(true).HasName("IX_Taxonomy_ArticleId");
            builder.Entity<TaxonomyState>().HasIndex(x => x.ParentId).IsUnique(false).HasName("IX_Taxonomy_ParentId");
            builder.Entity<TaxonomyState>().HasOne(x => x.Component).WithOne(x => x.Taxonomy).HasForeignKey<TaxonomyState>(x => x.Id);
        }
    }
}
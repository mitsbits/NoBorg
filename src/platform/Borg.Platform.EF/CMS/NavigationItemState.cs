using Borg.CMS;
using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public class NavigationItemState : IEntity<int>
    {
        public int Id { get; set; }
        public string GroupCode { get; set; }
        public string Path { get; set; }
        public string Display { get; set; }
        public NavigationItemType NavigationItemType { get; set; }
        public virtual TaxonomyState Taxonomy { get; set; }
        public virtual ComponentState Component => Taxonomy?.Component;
        public virtual ArticleState Article => Taxonomy?.Article;
    }

    public class NavigationItemStateMap : EntityMap<NavigationItemState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<NavigationItemState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<NavigationItemState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<NavigationItemState>().Property(x => x.GroupCode).IsRequired().HasMaxLength(256).HasDefaultValue("");
            builder.Entity<NavigationItemState>().Property(x => x.Display).IsRequired().HasMaxLength(1024).HasDefaultValue("").IsUnicode();
            builder.Entity<NavigationItemState>().HasIndex(x => x.GroupCode).IsUnique(false).HasName("IX_Navigation_GroupCode");
            builder.Entity<NavigationItemState>().HasOne(x => x.Taxonomy).WithOne(x => x.NavigationItem).HasForeignKey<NavigationItemState>(x => x.Id);
            builder.Entity<NavigationItemState>().Ignore(x => x.Component);
            builder.Entity<NavigationItemState>().Ignore(x => x.Article);
        }
    }
}
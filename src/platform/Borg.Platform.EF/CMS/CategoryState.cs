using System.Collections.Generic;
using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public class CategoryState : IEntity<int>
    {
        public int Id { get; set; }
        public string FriendlyName { get; set; }
        public string Slug { get; set; }
        public int GroupingId { get; set; }
        public virtual ComponentState Component { get; set; }
        public virtual CategoryGroupingState Grouping { get; set; }
        public virtual TaxonomyState Taxonomy { get; set; }
        public virtual ArticleState Article => Taxonomy?.Article;
        internal virtual ICollection<CategoryComponentAssociationState> CategoryComponentAssociations { get; set; } = new HashSet<CategoryComponentAssociationState>();
    }

    public class CategoryStateeMap : EntityMap<CategoryState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CategoryState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<CategoryState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<CategoryState>().Property(x => x.Slug).IsRequired().HasMaxLength(1024).HasDefaultValue("").IsUnicode(false);
            builder.Entity<CategoryState>().HasOne(x => x.Component).WithOne(x => x.Category).HasForeignKey<CategoryState>(x => x.Id);
            builder.Entity<CategoryState>().HasOne(x => x.Taxonomy).WithOne(x => x.Category).HasForeignKey<CategoryState>(x => x.Id);
            builder.Entity<CategoryState>().HasOne(x => x.Grouping).WithMany(x => x.Categories)
                .HasForeignKey(x => x.GroupingId).HasConstraintName("FK_CategoryGroupings_Categories");
        }
    }
}
using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Borg.Platform.EF.CMS
{
    public class ArticleState : IEntity<int>
    {
        public int Id { get; protected set; }
        public virtual ComponentState Component { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        internal virtual TaxonomyState Taxonomy { get; set; }
        public virtual ICollection<ArticleTagState> ArticleTags { get; } = new List<ArticleTagState>();
        public virtual PageMetadataState PageMetadata { get; set; }

        [NotMapped]
        public virtual IEnumerable<TagState> Tags => ArticleTags.Select(e => e.Tag);
    }

    public class ArticleStateMap : EntityMap<ArticleState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ArticleState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<ArticleState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<ArticleState>().Property(x => x.Title).IsRequired().HasMaxLength(1024).HasDefaultValue("").IsUnicode();
            builder.Entity<ArticleState>().Property(x => x.Slug).IsRequired().HasMaxLength(1024).HasDefaultValue("").IsUnicode(false);
            builder.Entity<ArticleState>().Property(x => x.Slug).IsRequired().HasDefaultValue("").IsUnicode();
            builder.Entity<ArticleState>().HasIndex(x => x.Title).IsUnique(false).HasName("IX_Article_Title");
            builder.Entity<ArticleState>().HasOne(x => x.Component).WithOne(x => x.Article).HasForeignKey<ArticleState>(x => x.Id);
        }
    }
}
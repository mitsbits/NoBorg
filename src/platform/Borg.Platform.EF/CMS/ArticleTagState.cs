using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public class ArticleTagState
    {
        public int ArticleId { get; set; }
        public virtual ArticleState Article { get; set; }

        public int TagId { get; set; }
        public virtual TagState Tag { get; set; }
    }

    public class ArticleTagStateMap : EntityMap<ArticleTagState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ArticleTagState>().HasKey(t => new { t.ArticleId, t.TagId }).ForSqlServerIsClustered();
            builder.Entity<ArticleTagState>().HasOne(pt => pt.Article).WithMany(x => x.ArticleTags).HasForeignKey(x => x.ArticleId).HasConstraintName("FK_Articles_ArticleTags");
            builder.Entity<ArticleTagState>().HasOne(pt => pt.Tag).WithMany(x => x.ArticleTags).HasForeignKey(x => x.TagId).HasConstraintName("FK_Tags_ArticleTags");
        }
    }
}
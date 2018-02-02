using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public class ArticleTagState
    {
        public int ArticleId { get; protected set; }
        public virtual ArticleState Article { get; protected set; }

        public int TagId { get; protected set; }
        public virtual TagState Tag { get; protected set; }
    }

    public class ArticleTagStateMap : EntityMap<ArticleTagState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ArticleTagState>().HasKey(t => new { t.ArticleId, t.TagId }).ForSqlServerIsClustered();
            builder.Entity<ArticleTagState>().HasOne(pt => pt.Article).WithMany("ArticleTags").OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<ArticleTagState>().HasOne(pt => pt.Tag).WithMany("ArticleTags").OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
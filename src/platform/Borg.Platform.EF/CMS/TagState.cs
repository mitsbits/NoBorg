using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Borg.Platform.EF.CMS
{
    public class TagState : IEntity<int>
    {
        public int Id { get;  set; }
        public virtual ComponentState Component { get;  set; }
        public string Tag { get;  set; }
        public string TagNormalized { get;  set; }
        public string TagSlug { get;  set; }
        public virtual ICollection<ArticleTagState> ArticleTags { get; set; } = new List<ArticleTagState>();

        [NotMapped]
        public IEnumerable<ArticleState> Articles => ArticleTags.Select(e => e.Article);
    }

    public class TagStateMap : EntityMap<TagState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TagState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<TagState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<TagState>().Property(x => x.Tag).HasMaxLength(1024).IsRequired().HasDefaultValue("").IsUnicode();
            builder.Entity<TagState>().Property(x => x.TagNormalized).HasMaxLength(1024).IsRequired().HasDefaultValue("").IsUnicode();
            builder.Entity<TagState>().Property(x => x.TagSlug).HasMaxLength(1024).IsRequired().HasDefaultValue("").IsUnicode(false);
            builder.Entity<TagState>().HasIndex(x => x.Tag).IsUnique(true).HasName("IX_Tag_Tag");
            builder.Entity<TagState>().HasIndex(x => x.TagNormalized).IsUnique(true).HasName("IX_Tag_TagNormalized");
            builder.Entity<TagState>().HasOne(x => x.Component).WithOne(x => x.Tag).HasForeignKey<TagState>(x => x.Id);
        }
    }
}
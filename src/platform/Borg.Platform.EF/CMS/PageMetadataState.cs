using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public class PageMetadataState : IEntity<int>
    {
        public int Id { get; set; }
        public string HtmlMetaJsonText { get; set; }
        public int? PrimaryImageDocumentId { get; set; }
        public int? PrimaryImageFileId { get; set; }

        public virtual ComponentState Component { get; set; }
        internal virtual ArticleState Article { get; set; }
    }

    public partial class ComponentState
    {
        internal PageMetadataState PageMetadata { get; set; }
    }

    public class PageMetadataStateMap : EntityMap<PageMetadataState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PageMetadataState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<PageMetadataState>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<PageMetadataState>().Property(x => x.HtmlMetaJsonText).IsRequired(false).HasDefaultValue("").IsUnicode();
            builder.Entity<PageMetadataState>().HasOne(x => x.Component).WithOne(x => x.PageMetadata).HasForeignKey<PageMetadataState>(x => x.Id).HasConstraintName("FK_Components_PageMetadatas");
            builder.Entity<PageMetadataState>().HasOne(x => x.Article).WithOne(x => x.PageMetadata).HasForeignKey<PageMetadataState>(x => x.Id).HasConstraintName("FK_Articles_PageMetadatas");
        }
    }
}
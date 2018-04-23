using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.Documents.Data
{
    public class MimeTypeGroupingExtensionState
    {
        public int MimeTypeGroupingId { get; set; }
        public string Extension { get; set; }

        public virtual MimeTypeGroupingState MimeTypeGrouping { get; set; }
    }

    public class MimeTypeGroupingExtensionStateMap : EntityMap<MimeTypeGroupingExtensionState, DocumentsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MimeTypeGroupingExtensionState>().HasKey(x => new { x.MimeTypeGroupingId, x.Extension }).ForSqlServerIsClustered();
            builder.Entity<MimeTypeGroupingExtensionState>().Property(x => x.Extension).IsRequired().HasMaxLength(64).IsUnicode(true);
            builder.Entity<MimeTypeGroupingExtensionState>().HasOne(x => x.MimeTypeGrouping).WithMany(x => x.Extensions)
                .HasForeignKey(x => x.MimeTypeGroupingId).HasConstraintName("FK_Grouping_Extension");
        }
    }
}
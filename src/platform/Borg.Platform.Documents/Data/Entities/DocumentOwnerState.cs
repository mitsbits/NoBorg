using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;
using System;

namespace Borg.Platform.Documents.Data.Entities
{
    public class DocumentOwnerState
    {
        public int DocumentId { get; set; }
        public string Owner { get; set; }
        public DateTimeOffset AssociatedOn { get; set; } = DateTimeOffset.UtcNow;

        internal virtual DocumentState Document { get; set; }
    }

    public class DocumentOwnerStateMap : EntityMap<DocumentOwnerState, DocumentsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DocumentOwnerState>().HasKey(x => new { DoecumentId = x.DocumentId, x.Owner }).ForSqlServerIsClustered();
            builder.Entity<DocumentOwnerState>().Property(x => x.DocumentId).ValueGeneratedNever();
            builder.Entity<DocumentOwnerState>().Property(x => x.Owner).IsRequired().HasMaxLength(256).IsUnicode(false);
            builder.Entity<DocumentOwnerState>().Property(x => x.AssociatedOn).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            builder.Entity<DocumentOwnerState>().HasIndex(x => x.AssociatedOn).IsUnique(false)
                .HasName("IX_DocumentOwnerState_AssociatedOn");
            builder.Entity<DocumentOwnerState>().HasOne(x => x.Document).WithMany(x => x.Owners)
                .HasForeignKey(x => x.DocumentId).HasConstraintName("FK_Document_Owners");
        }
    }
}
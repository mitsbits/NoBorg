using System;
using Borg.Platform.EF;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.Documents.Data
{
    public class DocumentCheckOutState
    {
        public int DocumentId { get; set; }
        public string CheckedOutBy { get; set; }

        public DateTimeOffset CheckedOutOn { get; set; } = DateTimeOffset.UtcNow;
        public int CheckOutVersion { get; set; }
        public bool CheckedIn { get; set; } = false;
        public DateTimeOffset? CheckedinOn { get; set; } = DateTimeOffset.UtcNow;

        [CanBeNull]
        public string CheckedInBy { get; set; }

        internal virtual DocumentState Document { get; set; }
    }

    public class DocumentCheckOutStateMap : EntityMap<DocumentCheckOutState, DocumentsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DocumentCheckOutState>().HasKey(x => new { DoecumentId = x.DocumentId, x.CheckedOutBy, x.CheckedOutOn }).ForSqlServerIsClustered();
            builder.Entity<DocumentCheckOutState>().Property(x => x.DocumentId).ValueGeneratedNever();
            builder.Entity<DocumentCheckOutState>().Property(x => x.CheckedOutBy).IsRequired().HasMaxLength(256).IsUnicode(false);
            builder.Entity<DocumentCheckOutState>().Property(x => x.CheckedOutOn).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            builder.Entity<DocumentCheckOutState>().Property(x => x.CheckOutVersion).IsRequired();
            builder.Entity<DocumentCheckOutState>().Property(x => x.CheckedInBy).IsRequired(false).HasMaxLength(256).IsUnicode(false);
            builder.Entity<DocumentCheckOutState>().HasIndex(x => x.CheckedOutOn).IsUnique(false).HasName("IX_DocumentCheckOutState_CheckedOutOn");
            builder.Entity<DocumentCheckOutState>().HasOne(x => x.Document).WithMany(x => x.CheckOuts).HasForeignKey(x => x.DocumentId).HasConstraintName("FK_Documents_CheckOuts");
        }
    }
}
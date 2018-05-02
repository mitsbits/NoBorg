using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Borg.Platform.EF.Instructions;

namespace Borg.Platform.Documents.Data.Entities
{
    public class MimeTypeState
    {
        public string Extension { get; set; }
        public string MimeType { get; set; }
        internal virtual ICollection<FileState> Files { get; set; }
    }

    public class MimeTypeStateMap : EntityMap<MimeTypeState, DocumentsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MimeTypeState>().HasKey(x => x.Extension).ForSqlServerIsClustered();
            builder.Entity<MimeTypeState>().Property(x => x.Extension).IsRequired().HasMaxLength(64).IsUnicode(true).ValueGeneratedNever();
            builder.Entity<MimeTypeState>().Property(x => x.MimeType).IsRequired().HasMaxLength(256).IsUnicode(true).ValueGeneratedNever();
            builder.Entity<MimeTypeState>().HasMany(x => x.Files).WithOne(x => x.MimeTypeState)
                .HasForeignKey(x => x.Extension).HasConstraintName("FK_MimeTypes_Records");
        }
    }
}
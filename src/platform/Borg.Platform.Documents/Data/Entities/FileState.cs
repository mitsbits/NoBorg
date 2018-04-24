using Borg.Infra.DDD.Contracts;
using Borg.Infra.Storage.Contracts;
using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;
using System;

namespace Borg.Platform.Documents.Data.Entities
{
    public class FileState : IEntity<int>, IFileSpec<int>
    {
        public int Id { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastWrite { get; set; }
        public DateTime? LastRead { get; set; }
        public long SizeInBytes { get; set; }
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public virtual VersionState VersionState { get; set; }
        internal virtual MimeTypeState MimeTypeState { get; set; }
    }

    public class FileStateMap : EntityMap<FileState, DocumentsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasSequence<int>("FilesSQC", "assets")
                .StartsAt(1)
                .IncrementsBy(1);

            builder.Entity<FileState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<FileState>().Property(x => x.Id).HasDefaultValueSql<int>("NEXT VALUE FOR assets.FilesSQC");
            builder.Entity<FileState>().Property(x => x.CreationDate).IsRequired().HasDefaultValueSql<DateTime>("GetUtcDate()");
            builder.Entity<FileState>().Property(x => x.LastWrite).IsRequired().HasDefaultValueSql<DateTime>("GetUtcDate()");
            builder.Entity<FileState>().Property(x => x.Name).HasMaxLength(512).IsRequired().HasDefaultValue<string>("");
            builder.Entity<FileState>().Property(x => x.FullPath).HasMaxLength(1024).IsUnicode(true).IsRequired().HasDefaultValue<string>("");
            builder.Entity<FileState>().Property(x => x.SizeInBytes).IsRequired().HasDefaultValueSql<long>("0");
            builder.Entity<FileState>().HasOne(x => x.VersionState).WithOne(x => x.FileState)
                .HasForeignKey<VersionState>(x => x.FileRecordId).HasConstraintName("FK_Version_File");
            builder.Entity<FileState>().Property(x => x.MimeType).HasMaxLength(256).IsRequired().HasDefaultValue<string>("");
            builder.Entity<FileState>().HasIndex(x => x.FullPath).HasName("IX_File_FullPath");
            builder.Entity<FileState>().Property(x => x.Extension).IsRequired().HasMaxLength(64).HasDefaultValue<string>("");
            builder.Entity<FileState>().HasIndex(x => x.Extension).HasName("IX_FileRecord_Extension").IsUnique(false);
        }
    }
}
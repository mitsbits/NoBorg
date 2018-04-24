using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.Documents.Data.Entities
{
    public class VersionState : IEntity<int>
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int AssetRecordId { get; set; }
        public virtual AssetState AssetState { get; set; }
        public int FileRecordId { get; set; }
        public virtual FileState FileState { get; set; }
    }

    public class VersionStateMap : EntityMap<VersionState, DocumentsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasSequence<int>("VersionsSQC", "assets")
                .StartsAt(1)
                .IncrementsBy(1);
            builder.Entity<VersionState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<VersionState>().Property(x => x.Id).HasDefaultValueSql<int>("NEXT VALUE FOR assets.VersionsSQC");
            builder.Entity<VersionState>().Property(x => x.Version).IsRequired().HasDefaultValueSql<int>("0");
            builder.Entity<VersionState>().HasIndex(x => x.Version).HasName("IX_Version_Version");
            builder.Entity<VersionState>().HasOne(x => x.AssetState).WithMany(x => x.Versions)
                .HasForeignKey(x => x.AssetRecordId).HasConstraintName("FK_Asset_Version");
            builder.Entity<VersionState>().HasIndex(x => x.FileRecordId).HasName("IX_Version_FileRecordId").IsUnique(false);
            builder.Entity<VersionState>().HasIndex(x => new { x.AssetRecordId, x.Version, x.FileRecordId }).IsUnique().HasName("PK_Version_Asset");
        }
    }
}
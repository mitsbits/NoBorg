using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.Assets.Data
{
    public class AssetsDbContext : DbContext
    {
        public AssetsDbContext(DbContextOptions<AssetsDbContext> option) : base(option)
        {

        }

        public DbSet<FileRecord> FileRecords { get; set; }
        public DbSet<VersionRecord> VersionRecords { get; set; }
        public DbSet<AssetRecord> AssetRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasSequence<int>("AssetsSQC", "assets")
                .StartsAt(1)
                .IncrementsBy(1);

            builder.HasSequence<int>("VersionsSQC", "assets")
                .StartsAt(1)
                .IncrementsBy(1);


            builder.HasSequence<int>("FilesSQC", "assets")
                .StartsAt(1)
                .IncrementsBy(1);




            builder.Entity<FileRecord>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<FileRecord>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<FileRecord>().Property(x => x.Id).HasDefaultValueSql("NEXT VALUE FOR assets.FilesSQC");
            builder.Entity<FileRecord>().Property(x => x.CreationDate).IsRequired().HasDefaultValueSql("GetUtcDate()");
            builder.Entity<FileRecord>().Property(x => x.LastWrite).IsRequired().HasDefaultValueSql("GetUtcDate()");
            builder.Entity<FileRecord>().Property(x => x.Name).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<FileRecord>().Property(x => x.FullPath).HasMaxLength(1024).IsRequired().HasDefaultValue("");
            builder.Entity<FileRecord>().Property(x => x.SizeInBytes).IsRequired().HasDefaultValueSql("0");
            builder.Entity<FileRecord>().HasOne(x => x.VersionRecord).WithOne(x => x.FileRecord)
                .HasForeignKey<VersionRecord>(x => x.FileRecordId).HasConstraintName("FK_Version_File");
            builder.Entity<FileRecord>().Property(x => x.MimeType).HasMaxLength(256).IsRequired().HasDefaultValue("");
            builder.Entity<FileRecord>().HasIndex(x => x.FullPath).HasName("IX_File_FullPath");

            builder.Entity<VersionRecord>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<VersionRecord>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<VersionRecord>().Property(x => x.Id).HasDefaultValueSql("NEXT VALUE FOR assets.VersionsSQC");
            builder.Entity<VersionRecord>().Property(x => x.Version).IsRequired().HasDefaultValueSql("0");
            builder.Entity<VersionRecord>().HasIndex(x => x.Version).HasName("IX_Version_Version");
            builder.Entity<VersionRecord>().HasOne(x => x.AssetRecord).WithMany(x => x.Versions)
                .HasForeignKey(x => x.AssetRecordId).HasConstraintName("FK_Asset_Version");
            builder.Entity<VersionRecord>().HasIndex(x => x.FileRecordId).HasName("IX_Version_FileRecordId");
            builder.Entity<VersionRecord>().HasIndex(x => new{ x.AssetRecordId, x.Version}).IsUnique().HasName("PK_Version_Asset");

            builder.Entity<AssetRecord>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<AssetRecord>().Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<AssetRecord>().Property(x => x.Id).HasDefaultValueSql("NEXT VALUE FOR assets.AssetsSQC");
            builder.Entity<AssetRecord>().Property(x => x.Name).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<AssetRecord>().Property(x => x.CurrentVersion).IsRequired().HasDefaultValueSql("0");
            builder.Entity<AssetRecord>().Property(x => x.DocumentState).IsRequired();

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                entityType.Relational().Schema = "assets";
            }
        }
    }
}
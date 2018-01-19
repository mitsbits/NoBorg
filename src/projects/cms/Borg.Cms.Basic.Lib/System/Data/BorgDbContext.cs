using Borg.Cms.Basic.Lib.Features.Content;
using Borg.Cms.Basic.Lib.Features.Device;
using Borg.Cms.Basic.Lib.Features.Navigation;
using Borg.MVC.BuildingBlocks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Borg.Cms.Basic.Lib.System.Data
{
    public class BorgDbContext : DbContext
    {
        public BorgDbContext(DbContextOptions<BorgDbContext> options)
            : base(options)
        {
        }


        public DbSet<NavigationItemRecord> NavigationItemRecords { get; set; }
        public DbSet<DeviceRecord> DeviceRecords { get; set; }
        public DbSet<SectionRecord> SectionRecords { get; set; }
        public DbSet<SlotRecord> SlotRecords { get; set; }
        public DbSet<ContentItemRecord> ContentItemRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<NavigationItemRecord>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<NavigationItemRecord>().Ignore(x => x.Depth);
            builder.Entity<NavigationItemRecord>().Property(x => x.Display).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<NavigationItemRecord>().Property(x => x.Group).HasMaxLength(3).IsRequired().HasDefaultValue("BSE");
            builder.Entity<NavigationItemRecord>().Property(x => x.ParentId).IsRequired().HasDefaultValue(0);
            builder.Entity<NavigationItemRecord>().Property(x => x.Path).HasMaxLength(512).HasDefaultValue("/");
            builder.Entity<NavigationItemRecord>().HasOne(n => n.ContentItemRecord).WithOne(c => c.NavigationItemRecord)
                .HasForeignKey<NavigationItemRecord>(x => x.ContentItemRecordId).HasConstraintName("FK_Navigation_Content");

            builder.Entity<DeviceRecord>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<DeviceRecord>().Property(x => x.FriendlyName).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<DeviceRecord>().Property(x => x.RenderScheme).HasMaxLength(512).IsRequired().HasDefaultValue(DeviceRenderScheme.UnSet);
            builder.Entity<DeviceRecord>().Property(x => x.Layout).HasMaxLength(512).IsRequired().HasDefaultValue("");

            builder.Entity<SectionRecord>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<SectionRecord>().HasIndex(x => x.DeviceId).HasName("IX_Section_DeviceId");
            builder.Entity<SectionRecord>().HasIndex(x => x.Identifier).HasName("IX_Section_Identifier");
            builder.Entity<SectionRecord>().HasOne(p => p.Device).WithMany(b => b.Sections)
                .HasForeignKey(p => p.DeviceId).HasConstraintName("FK_Device_Section");
            builder.Entity<SectionRecord>().Property(x => x.FriendlyName).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<SectionRecord>().Property(x => x.Identifier).HasMaxLength(512).IsRequired().HasDefaultValue("");

            builder.Entity<SlotRecord>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<SlotRecord>().HasIndex(x => x.SectionId).HasName("IX_Slot_SectionId");
            builder.Entity<SlotRecord>().HasIndex(x => x.Ordinal).HasName("IX_Slot_Ordinal");
            builder.Entity<SlotRecord>().HasIndex(x => x.ModuleTypeName).HasName("IX_Slot_ModuleTypeName");
            builder.Entity<SlotRecord>().HasIndex(x => x.ModuleGender).HasName("IX_Slot_ModuleGender");
            builder.Entity<SlotRecord>().HasOne(p => p.Section).WithMany(b => b.Slots)
                .HasForeignKey(p => p.SectionId).HasConstraintName("FK_Section_Slot");
            builder.Entity<SlotRecord>().Property(x => x.ModuleDecriptorJson).IsRequired().HasDefaultValue("");
            builder.Entity<SlotRecord>().Property(x => x.ModuleGender).IsRequired().HasMaxLength(64).HasDefaultValue("");
            builder.Entity<SlotRecord>().Property(x => x.ModuleTypeName).IsRequired().HasMaxLength(1024).HasDefaultValue("");

            builder.Entity<ContentItemRecord>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<ContentItemRecord>().Property(x => x.Title).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<ContentItemRecord>().Property(x => x.Slug).HasMaxLength(512).HasDefaultValue("");
            builder.Entity<ContentItemRecord>().Property(x => x.Subtitle).HasMaxLength(512).HasDefaultValue("");
            builder.Entity<ContentItemRecord>().Property(x => x.PublisheDate).IsRequired().HasDefaultValueSql("GetUtcDate()");

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                entityType.Relational().Schema = "borg";
            }
        }
    }
}
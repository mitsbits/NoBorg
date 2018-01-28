using Borg.Cms.Basic.Lib.Features.Content;
using Borg.Cms.Basic.Lib.Features.Device;
using Borg.MVC.BuildingBlocks;
using Borg.Platform.EF.CMS;
using Microsoft.EntityFrameworkCore;

namespace Borg.Cms.Basic.Lib.System.Data
{
    public class BorgDbContext : DbContext
    {
        public BorgDbContext(DbContextOptions<BorgDbContext> options)
            : base(options)
        {
        }

        public DbSet<ContentItemRecord> ContentItemRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



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
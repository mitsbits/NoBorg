using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public class InstanceBlockState : IEntity<string>
    {
        public string Id { get; protected set; }
        public string Display { get; protected set; }
        public string IconClass { get; set; }
        public string JsonText { get; set; }
    }

    public class ConfigurationBlockStateMap : EntityMap<InstanceBlockState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<InstanceBlockState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<InstanceBlockState>().Property(x => x.Id).HasMaxLength(2048).IsRequired().HasDefaultValue("");
            builder.Entity<InstanceBlockState>().Property(x => x.IconClass).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<InstanceBlockState>().Property(x => x.Display).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<InstanceBlockState>().HasIndex(x => x.Display).HasName("IX_ConfigurationBlock_Display");
        }
    }
}
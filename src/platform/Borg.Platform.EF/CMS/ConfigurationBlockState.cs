using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public class ConfigurationBlockState : IEntity<string>
    {
        public string Id { get;  set; }
        public string Display { get;  set; }

        public string IconClass { get; set; }
        public string JsonText { get; set; }
    }

    public class ConfigurationBlockStateMap : EntityMap<ConfigurationBlockState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ConfigurationBlockState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<ConfigurationBlockState>().Property(x => x.Id).HasMaxLength(2048).IsRequired().HasDefaultValue("");
            builder.Entity<ConfigurationBlockState>().Property(x => x.IconClass).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<ConfigurationBlockState>().Property(x => x.Display).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<ConfigurationBlockState>().HasIndex(x => x.Display).HasName("IX_ConfigurationBlock_Display");
        }
    }
}
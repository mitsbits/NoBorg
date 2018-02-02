using Borg.CMS.BuildingBlocks;
using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS
{
    public class DeviceState : IEntity<int>
    {
        public int Id { get; set; }
        public string FriendlyName { get; set; }
        public string Layout { get; set; }
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;
        public string Theme { get; set; }
        public ICollection<SectionState> Sections { get; set; } = new HashSet<SectionState>();
        internal virtual ComponentDeviceState ComponentDevice { get; set; }
    }

    public class DeviceStateMap : EntityMap<DeviceState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DeviceState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<DeviceState>().Property(x => x.FriendlyName).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<DeviceState>().Property(x => x.RenderScheme).HasMaxLength(512).IsRequired().HasDefaultValue(DeviceRenderScheme.UnSet);
            builder.Entity<DeviceState>().Property(x => x.Layout).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<DeviceState>().Property(x => x.Theme).HasMaxLength(256).IsUnicode(false).IsRequired(false).HasDefaultValue("");
        }
    }
}
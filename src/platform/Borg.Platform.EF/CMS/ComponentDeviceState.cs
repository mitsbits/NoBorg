﻿using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public class ComponentDeviceState
    {
        public int ComponentId { get; set; }
        public virtual ComponentState Component { get; set; }
        public int DeviceId { get; set; }
        public virtual DeviceState Device { get; set; }
    }

    public partial class ComponentState
    {
        internal ComponentDeviceState ComponentDevice { get; set; }
    }

    public class ComponentDeviceStateMap : EntityMap<ComponentDeviceState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ComponentDeviceState>().HasKey(t => new { t.ComponentId, t.DeviceId }).ForSqlServerIsClustered();
            builder.Entity<ComponentDeviceState>().HasOne(pt => pt.Component).WithOne(x => x.ComponentDevice);
            builder.Entity<ComponentDeviceState>().HasOne(pt => pt.Device).WithOne(x => x.ComponentDevice);
            builder.Entity<ComponentDeviceState>().HasIndex(x => x.ComponentId).IsUnique(false).HasName("IX_ComponentDeviceState_ComponentId");
            builder.Entity<ComponentDeviceState>().HasIndex(x => x.DeviceId).IsUnique(false).HasName("IX_ComponentDeviceState_DeviceId");
        }
    }
}
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.CMS
{
    public class ComponentDeviceState
    {
        public int ComponentId { get; set; }
        public virtual ComponentState Component { get;  set; }
        public int DeviceId { get; set; }
        public virtual DeviceState Device { get;  set; }
    }

    public class ComponentDeviceStateMap : EntityMap<ComponentDeviceState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ComponentDeviceState>().HasKey(t => new { t.ComponentId, t.DeviceId }).ForSqlServerIsClustered();
            builder.Entity<ComponentDeviceState>().HasOne(pt => pt.Component).WithOne(x => x.ComponentDevice);
            builder.Entity<ComponentDeviceState>().HasOne(pt => pt.Device).WithOne(x => x.ComponentDevice);

        }
    }
}
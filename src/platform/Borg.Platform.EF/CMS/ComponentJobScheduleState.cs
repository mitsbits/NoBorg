using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Borg.Platform.EF.Instructions;

namespace Borg.Platform.EF.CMS
{
    public class ComponentJobScheduleState
    {
        public int ComponentId { get; set; }
        public int ScheduleId { get; set; }
        internal virtual ComponentState Component { get; set; }
    }

    public partial class ComponentState
    {
        internal ICollection<ComponentJobScheduleState> ComponentJobSchedules { get; set; } = new HashSet<ComponentJobScheduleState>();
    }

    public class ComponentJobScheduleStateMap : EntityMap<ComponentJobScheduleState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ComponentJobScheduleState>().HasKey(x => new { x.ComponentId, x.ScheduleId }).ForSqlServerIsClustered();
            builder.Entity<ComponentJobScheduleState>().HasOne(x => x.Component).WithMany(x => x.ComponentJobSchedules)
                .HasForeignKey(x => x.ComponentId).HasConstraintName("FK_Component_JobSchedules");
        }
    }
}
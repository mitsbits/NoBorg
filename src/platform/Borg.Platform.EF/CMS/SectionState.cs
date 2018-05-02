using Borg.CMS.BuildingBlocks;
using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Borg.Platform.EF.Instructions;

namespace Borg.Platform.EF.CMS
{
    public class SectionState : IEntity<int>
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public int DeviceId { get; set; }
        public string FriendlyName { get; set; }
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;
        public ICollection<SlotState> Slots { get; set; } = new HashSet<SlotState>();
        public virtual DeviceState Device { get; set; }
    }

    public class SectionStateMap : EntityMap<SectionState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SectionState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<SectionState>().HasIndex(x => x.DeviceId).HasName("IX_Section_DeviceId");
            builder.Entity<SectionState>().HasIndex(x => x.Identifier).HasName("IX_Section_Identifier");
            builder.Entity<SectionState>().HasOne(p => p.Device).WithMany(b => b.Sections)
                .HasForeignKey(p => p.DeviceId).HasConstraintName("FK_Device_Section");
            builder.Entity<SectionState>().Property(x => x.FriendlyName).HasMaxLength(512).IsRequired().HasDefaultValue("");
            builder.Entity<SectionState>().Property(x => x.Identifier).HasMaxLength(512).IsRequired().HasDefaultValue("");
        }
    }
}
using System;
using Borg.CMS;
using Borg.CMS.BuildingBlocks;
using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Borg.Platform.EF.CMS
{
    public class SlotState : IEntity<int>
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public int Ordinal { get; set; }
        public int SectionId { get; set; }
        public string ModuleDecriptorJson { get; set; }
        public string ModuleGender { get; set; }
        public string ModuleTypeName { get; set; }
        public virtual SectionState Section { get; set; }

        public virtual (SectionSlotInfo slotInfo, ModuleRenderer renderer) Module()
        {
            var renderer = JsonConvert.DeserializeObject<ModuleRenderer>(ModuleDecriptorJson);
            var slot = new SectionSlotInfo(Section?.Identifier, IsEnabled, Ordinal);
            return ValueTuple.Create(slot, renderer);
        }

        public virtual (SectionSlotInfo slotInfo, ModuleRenderer renderer) Module(string sectionIdentifier)
        {
            var renderer = JsonConvert.DeserializeObject<ModuleRenderer>(ModuleDecriptorJson);
            var slot = new SectionSlotInfo(sectionIdentifier, IsEnabled, Ordinal);
            return ValueTuple.Create(slot, renderer);
        }
    }

    public class SlotStateMap : EntityMap<SlotState, CmsDbContext>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SlotState>().HasKey(x => x.Id).ForSqlServerIsClustered();
            builder.Entity<SlotState>().HasIndex(x => x.SectionId).HasName("IX_Slot_SectionId");
            builder.Entity<SlotState>().HasIndex(x => x.Ordinal).HasName("IX_Slot_Ordinal");
            builder.Entity<SlotState>().HasIndex(x => x.ModuleTypeName).HasName("IX_Slot_ModuleTypeName");
            builder.Entity<SlotState>().HasIndex(x => x.ModuleGender).HasName("IX_Slot_ModuleGender");
            builder.Entity<SlotState>().HasOne(p => p.Section).WithMany(b => b.Slots)
                .HasForeignKey(p => p.SectionId).HasConstraintName("FK_Section_Slot");
            builder.Entity<SlotState>().Property(x => x.ModuleDecriptorJson).IsRequired().HasDefaultValue("");
            builder.Entity<SlotState>().Property(x => x.ModuleGender).IsRequired().HasMaxLength(64).HasDefaultValue("");
            builder.Entity<SlotState>().Property(x => x.ModuleTypeName).IsRequired().HasMaxLength(1024).HasDefaultValue("");
        }
    }
}
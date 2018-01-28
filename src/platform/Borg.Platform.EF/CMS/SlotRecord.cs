using System;
using Borg.CMS;
using Borg.CMS.BuildingBlocks;
using Borg.Infra.DDD.Contracts;
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
}
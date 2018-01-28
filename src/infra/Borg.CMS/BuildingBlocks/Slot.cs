using Borg.CMS.BuildingBlocks.Contracts;
using Borg.Infra;

namespace Borg.CMS.BuildingBlocks
{
    public class Slot : ISlot
    {
        public Slot(SectionSlotInfo sectionSlotInfo, ModuleRenderer module)
        {
            Preconditions.NotNull(sectionSlotInfo, nameof(sectionSlotInfo));
            Preconditions.NotNull(module, nameof(module));
            SectionSlotInfo = sectionSlotInfo;
            Module = module;
        }

        ISectionSlotInfo ISlot.SectionSlotInfo => SectionSlotInfo;

        public SectionSlotInfo SectionSlotInfo { get; private set; }
        public ModuleRenderer Module { get; private set; }

        public Slot NewModule(ModuleRenderer module)
        {
            return new Slot(SectionSlotInfo, module);
        }

        public Slot NewSlotInfo(SectionSlotInfo sectionSlotInfo)
        {
            return new Slot(sectionSlotInfo, Module);
        }
    }
}
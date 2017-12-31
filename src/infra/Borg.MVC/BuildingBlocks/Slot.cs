using Borg.MVC.BuildingBlocks.Contracts;

namespace Borg.MVC.BuildingBlocks
{
    public class Slot : ISlot
    {
        public Slot(SectionSlotInfo sectionSlotInfo, BaseModule module)
        {
            SectionSlotInfo = sectionSlotInfo;
            Module = module;
        }

        ISectionSlotInfo ISlot.SectionSlotInfo => SectionSlotInfo;

        public SectionSlotInfo SectionSlotInfo { get; private set; }
        public BaseModule Module { get; private set; }

        public Slot NewModule(BaseModule module)
        {
            return new Slot(SectionSlotInfo, module);
        }

        public Slot NewSlotInfo(SectionSlotInfo sectionSlotInfo)
        {
            return new Slot(sectionSlotInfo, Module);
        }
    }
}
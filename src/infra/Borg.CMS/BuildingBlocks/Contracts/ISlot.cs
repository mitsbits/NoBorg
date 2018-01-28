namespace Borg.CMS.BuildingBlocks.Contracts
{
    public interface ISlot
    {
        ISectionSlotInfo SectionSlotInfo { get; }

        ModuleRenderer Module { get; }
    }
}
namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface ISlot
    {
        ISectionSlotInfo SectionSlotInfo { get; }

        ModuleRenderer Module { get; }
    }
}
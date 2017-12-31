namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface ISlot
    {
        ISectionSlotInfo SectionSlotInfo { get; }

        BaseModule Module { get; }
    }
}
namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface ISectionSlotInfo
    {
        string SectionIdentifier { get; }
        bool Enabled { get; }
        int Ordinal { get; }
    }
}
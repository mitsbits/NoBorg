namespace Borg.CMS.BuildingBlocks.Contracts
{
    public interface ISectionSlotInfo
    {
        string SectionIdentifier { get; }
        bool Enabled { get; }
        int Ordinal { get; }
    }
}
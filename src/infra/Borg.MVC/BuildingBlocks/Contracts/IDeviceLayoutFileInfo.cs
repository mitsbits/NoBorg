namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IDeviceLayoutFileInfo
    {
        string FullPath { get; set; }
        string[] SectionIdentifiers { get; set; }
    }
}
namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IDeviceLayoutFileInfo
    {
        string Theme { get; set; }
        string FullPath { get; set; }
        string[] SectionIdentifiers { get; set; }
        bool MatchesPath(string absolutePath);
    }
}
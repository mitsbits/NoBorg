using Borg.MVC.BuildingBlocks.Contracts;

namespace Borg.MVC.BuildingBlocks
{
    public class DeviceLayoutFileInfo : IDeviceLayoutFileInfo
    {
        public string FullPath { get; set; }
        public string[] SectionIdentifiers { get; set; }
    }
}
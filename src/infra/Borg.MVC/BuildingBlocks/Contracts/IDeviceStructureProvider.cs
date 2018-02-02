using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IDeviceStructureProvider
    {
        Task<IDeviceStructureInfo> PageLayout(int id);

        Task<IDeviceStructureInfo> PageLayout(string layout);

        Task<IEnumerable<IDeviceStructureInfo>> PageLayouts();
    }
}
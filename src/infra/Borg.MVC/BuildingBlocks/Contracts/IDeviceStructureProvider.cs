using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Borg.MVC.BuildingBlocks.Contracts
{
  public  interface IDeviceStructureProvider
    {
        Task<IDeviceStructureInfo> PageLayout(int id);
    }
}

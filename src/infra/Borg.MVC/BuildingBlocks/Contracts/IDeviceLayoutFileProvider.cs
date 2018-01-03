﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IDeviceLayoutFileProvider
    {
        Task<IEnumerable<IDeviceLayoutFileInfo>> LayoutFiles();
    }
}
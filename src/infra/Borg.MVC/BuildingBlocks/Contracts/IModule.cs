﻿using System.Collections.Generic;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IModule<out TData>
        where TData : IDictionary<string, string>
    {
        string FriendlyName { get; }

        TData Parameters { get; }

        ModuleType ModuleType { get; }
    }
}
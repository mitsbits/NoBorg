using System.Collections.Generic;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IModuleRenderer<out TData> : IModuleDescriptor where TData : IDictionary<string, string>
    {
        TData Parameters { get; }
    }
}
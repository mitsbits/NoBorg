using System.Collections.Generic;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IModuleDescriptor<TModule, out TData> : IModuleDescriptor<TData> where TData : IDictionary<string, string> where TModule : IModule<TData>
    {
    }

    public interface IModuleDescriptor<out TData> : IModuleDescriptor where TData : IDictionary<string, string>
    {
        TData Parameters { get; }
    }

    public interface IModuleDescriptor
    {
        string FriendlyName { get; }
        string Summary { get; }
        string ModuleGroup { get; }
        ModuleGender ModuleGender { get; }
    }
}
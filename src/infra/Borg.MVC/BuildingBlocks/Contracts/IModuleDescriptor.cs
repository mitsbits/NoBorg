using System.Collections.Generic;
using System.Runtime.InteropServices;
using Borg.Infra.DTO;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IModuleDescriptor<TModule, out TData> : IModuleDescriptor where TData : IDictionary<string, string> where TModule : IModule<TData>
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

    public interface IModuleRenderer<out TData> : IModuleDescriptor where TData : IDictionary<string, string>
    {
        TData Parameters { get; }
    }

    public class ModuleRenderer 
    {
        public string FriendlyName { get; set; }
        public string Summary { get; set; }
        public string ModuleGroup { get; set; }
        public Tiding[] Parameters { get; set; }
    }
}
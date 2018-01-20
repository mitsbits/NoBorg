using System.Collections.Generic;

namespace Borg.MVC.PlugIns.Contracts
{
    public interface IPlugInHost
    {
        IEnumerable<IPluginDescriptor> PlugIns { get; }
    }
}
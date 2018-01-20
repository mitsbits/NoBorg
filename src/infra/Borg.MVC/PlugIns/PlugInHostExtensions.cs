using Borg.MVC.PlugIns.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Borg
{
    public static class PlugInHostExtensions
    {
        public static IEnumerable<TPlugIn> Specify<TPlugIn>(this IPlugInHost host) where TPlugIn : IPluginDescriptor
        {
            return host.PlugIns.Where(t => t.GetType().IsAssignableTo(typeof(TPlugIn))).Cast<TPlugIn>();
        }
    }
}
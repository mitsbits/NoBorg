using Borg.MVC.PlugIns.Contracts;
using Borg.MVC.PlugIns.Decoration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Borg
{
    public static class PlugInHostExtensions
    {
        public static IEnumerable<TPlugIn> SpecifyPlugins<TPlugIn>(this IPlugInHost host) where TPlugIn : IPluginDescriptor
        {
            return host.PlugIns.Where(t => t.GetType().IsAssignableTo(typeof(TPlugIn))).Cast<TPlugIn>();
        }

        public static IEnumerable<IPlugInArea> PlugInAreas(this IPlugInHost host)
        {
            return host.SpecifyPlugins<IPlugInArea>();
        }

        public static IServiceCollection RegisterDiscoveryServices(this IPluginServiceRegistration descriptor, IServiceCollection services)
        {
            var thisasmbl = Assembly.GetAssembly(descriptor.GetType());
            var registries = thisasmbl.GetTypes().Where(X => X.CustomAttributes.Any(at =>
                at.AttributeType.IsSealed && at.AttributeType.IsAssignableTo(typeof(ServiceAttribute))));
            foreach (var registry in registries)
            {
                var attr = registry.GetCustomAttributes();
                foreach (var att in attr)
                {
                    if (!(att is ServiceAttribute attts)) continue;
                    if (!att.GetType().IsSealed) continue;
                    var descr = new ServiceDescriptor(registry, attts.ServiceType, attts.ServiceLifetime);
                    services.Add(descr);
                }
            }
            return services;
        }
    }
}
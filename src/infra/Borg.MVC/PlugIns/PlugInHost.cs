using Borg.Infra.Services.AssemblyProvider;
using Borg.MVC.PlugIns.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Borg.MVC.PlugIns
{
    public class PlugInHost : IPlugInHost
    {
        private readonly List<IPluginDescriptor> _plugins = new List<IPluginDescriptor>();
        private readonly ILogger _logger;

        public PlugInHost(ILoggerFactory loggerFactory, IEnumerable<Assembly> assembliesToScan)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _logger.Info("Searching in {count} assemblies for plug ins", assembliesToScan.Count());
            var plugintypes = assembliesToScan.SelectMany(x => x.GetTypes())
                .Where(x => x.ImplementsInterface(typeof(IPluginDescriptor)) && x.IsSealed)
                .Distinct().ToList();

            _logger.Info("Found {count} plug ins", plugintypes.Count());

            foreach (var pluginDescriptor in plugintypes)
            {
                _plugins.Add(Activator.CreateInstance(pluginDescriptor) as IPluginDescriptor);
                _logger.Info("Adding {plugintype} plug ins", pluginDescriptor.FullName);
            }
        }

        public PlugInHost(ILoggerFactory loggerFactory, IEnumerable<IAssemblyProvider> assemblyproviders) : this(loggerFactory, assemblyproviders.SelectMany(x => x.GetAssemblies()).Distinct())
        {
        }

        public IEnumerable<IPluginDescriptor> PlugIns => _plugins;
    }
}
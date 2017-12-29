using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Borg.Infra.Services.AssemblyProvider
{
    public class DepedencyAssemblyProvider : IAssemblyProvider
    {
        protected ILogger Logger { get; }

        public DepedencyAssemblyProvider(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory != null ? loggerFactory.CreateLogger(GetType()) : NullLogger.Instance;
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            GetAssembliesFromDependencyContext(assemblies);
            return assemblies;
        }

        private void GetAssembliesFromDependencyContext(List<Assembly> assemblies)
        {
            Logger.Info("Discovering and loading assemblies from DependencyContext");

            foreach (CompilationLibrary compilationLibrary in DependencyContext.Default.CompileLibraries)
            {
                Assembly assembly = null;

                try
                {
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(compilationLibrary.Name));

                    if (!assemblies.Any(a => string.Equals(a.FullName, assembly.FullName, StringComparison.OrdinalIgnoreCase)))
                    {
                        assemblies.Add(assembly);
                        Logger.Info("Assembly '{0}' is discovered and loaded", assembly.FullName);
                    }
                }
                catch (Exception e)
                {
                    Logger.Warn("Error loading assembly '{0}'", compilationLibrary.Name);
                    Logger.Warn(e.ToString());
                }
            }
        }
    }
}
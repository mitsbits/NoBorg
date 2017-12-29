using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Borg.Infra.Services.AssemblyProvider
{
    public class ReferenceAssemblyProvider : IAssemblyProvider
    {
        protected ILogger Logger { get; }
        private readonly IEnumerable<AssemblyName> _assemblies;

        public ReferenceAssemblyProvider(ILoggerFactory loggerFactory, params Assembly[] assemblies)
        {
            Logger = loggerFactory != null ? loggerFactory.CreateLogger(GetType()) : NullLogger.Instance;
            if (assemblies == null || !assemblies.Any())
                _assemblies = (new[] { Assembly.GetExecutingAssembly().GetName() }.Union(Assembly.GetExecutingAssembly().GetReferencedAssemblies()));

            _assemblies = assemblies.Select(x => x.GetName());
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            GetAssembliesFromReferenceContext(assemblies);
            return assemblies;
        }

        private void GetAssembliesFromReferenceContext(List<Assembly> assemblies)
        {
            Logger.Info("Discovering and loading assemblies from ReferenceContext");

            foreach (var asmbl in _assemblies)
            {
                //if (this.IsCandidateCompilationLibrary(compilationLibrary))
                //{
                Assembly assembly = null;

                try
                {
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(asmbl);

                    if (!assemblies.Any(a => string.Equals(a.FullName, assembly.FullName, StringComparison.OrdinalIgnoreCase)))
                    {
                        assemblies.Add(assembly);
                        Logger.Info("Assembly '{0}' is discovered and loaded", assembly.FullName);
                    }
                }
                catch (Exception e)
                {
                    Logger.Warn("Error loading assembly '{0}'", asmbl);
                    Logger.Warn(e.ToString());
                }
                //}
            }
        }

        //private void GetAssembliesFromDependencyContext(List<Assembly> assemblies)
        //{
        //    Logger.Info("Discovering and loading assemblies from DependencyContext");

        //    foreach (CompilationLibrary compilationLibrary in DependencyContext.Default.CompileLibraries)
        //    {
        //        if (this.IsCandidateCompilationLibrary(compilationLibrary))
        //        {
        //            Assembly assembly = null;

        //            try
        //            {
        //                assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(compilationLibrary.Name));

        //                if (!assemblies.Any(a => string.Equals(a.FullName, assembly.FullName, StringComparison.OrdinalIgnoreCase)))
        //                {
        //                    assemblies.Add(assembly);
        //                    this.logger.LogInformation("Assembly '{0}' is discovered and loaded", assembly.FullName);
        //                }
        //            }

        //            catch (Exception e)
        //            {
        //                this.logger.LogWarning("Error loading assembly '{0}'", compilationLibrary.Name);
        //                this.logger.LogWarning(e.ToString());
        //            }
        //        }
        //    }
        //}
    }
}
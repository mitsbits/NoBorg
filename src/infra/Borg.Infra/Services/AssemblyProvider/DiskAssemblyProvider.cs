using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Borg.Infra.Services.AssemblyProvider
{
    public class DiskAssemblyProvider : IAssemblyProvider
    {
        protected ILogger Logger { get; }
        private readonly string _path;
        private readonly bool _includingSubpaths;

        public DiskAssemblyProvider(ILoggerFactory loggerFactory, string path, bool includingSubpaths = false)
        {
            Logger = loggerFactory != null ? loggerFactory.CreateLogger(GetType()) : NullLogger.Instance;
            _path = path;
            _includingSubpaths = includingSubpaths;
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            GetAssembliesFromPath(assemblies, _path, _includingSubpaths);
            return assemblies;
        }

        private void GetAssembliesFromPath(List<Assembly> assemblies, string path, bool includingSubpaths)
        {
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                Logger.Info("Discovering and loading assemblies from path '{0}'", path);

                foreach (var extensionPath in Directory.EnumerateFiles(path, "*.dll"))
                {
                    try
                    {
                        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(extensionPath);

                        assemblies.Add(assembly);
                        Logger.Info("Assembly '{0}' is discovered and loaded", assembly.FullName);
                    }
                    catch (Exception e)
                    {
                        Logger.Warn("Error loading assembly '{0}'", extensionPath);
                        Logger.Warn(e.ToString());
                    }
                }

                if (includingSubpaths)
                    foreach (string subpath in Directory.GetDirectories(path))
                        GetAssembliesFromPath(assemblies, subpath, true);
            }
            else
            {
                Logger.Warn(
                    string.IsNullOrEmpty(path)
                        ? "Discovering and loading assemblies from path skipped: path not provided"
                        : "Discovering and loading assemblies from path '{0}' skipped: path not found", path);
            }
        }
    }
}
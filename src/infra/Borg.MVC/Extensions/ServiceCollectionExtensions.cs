using Borg.Infra;
using Borg.Infra.Configuration.Contracts;
using Borg.Infra.Services.AssemblyProvider;
using Borg.Infra.Storage;
using Borg.Infra.Storage.Contracts;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Filters;
using Borg.MVC.Services.ServerResponses;
using Borg.MVC.Services.UserSession;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBorgFramework(this IServiceCollection services, IHostingEnvironment env, ISettingsProvider<StorageSettings> settings)
        {
            services.AddSingleton<ISerializer, JsonNetSerializer>();

            services.AddScoped<IPageOrchestrator<IPageContent, IDevice>, PageOrchestrator>();
            services.AddScoped<IPageOrchestrator, PageOrchestrator>();
            services.AddScoped<IDeviceAccessor<IDevice>, PageOrchestrator>();
            services.AddScoped<IPageContentAccessor<IPageContent>, PageOrchestrator>();
            services.AddBorgUserSession();

            services.AddBorgDefaultSlugifier();

            services.AddScoped<DeviceLayoutFilter>();

            services.AddScoped<IFileStorage>(s => new FolderFileStorage(Path.Combine(env.WebRootPath, settings.Config.Folder), s.GetService<ILoggerFactory>()));

            return services;
        }

        private static IServiceCollection AddBorgUserSession(this IServiceCollection services)
        {
            services.AddScoped<ISessionServerResponseProvider, TempDataResponseProvider>();
            services.AddScoped<IUserSession, BorgUserSession>();
            services.AddScoped<IContextAwareUserSession, BorgUserSession>();
            return services;
        }

        public static Assembly[] FireUpAssemblyScanners(this IServiceCollection services, ILoggerFactory loggerFactory, Func<Assembly, bool> filter = null)
        {
            var depedencyAssemblyProvider = new DepedencyAssemblyProvider(loggerFactory);
            var referenceAssemblyProvider = new ReferenceAssemblyProvider(loggerFactory);
            services.Add(new ServiceDescriptor(typeof(IAssemblyProvider), depedencyAssemblyProvider));
            services.Add(new ServiceDescriptor(typeof(IAssemblyProvider), referenceAssemblyProvider));

            var query = depedencyAssemblyProvider.GetAssemblies().Union(referenceAssemblyProvider.GetAssemblies()).Distinct();
            return filter == null ? query.ToArray() : query.Where(filter).ToArray();
        }
    }
}
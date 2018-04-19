using System;
using Borg.Infra;
using Borg.Infra.Storage;
using Borg.Infra.Storage.Contracts;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Filters;
using Borg.MVC.Services.ServerResponses;
using Borg.MVC.Services.UserSession;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Borg.Infra.Services.AssemblyProvider;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBorgFramework(this IServiceCollection services, IHostingEnvironment env, BorgSettings settings)
        {
            services.AddSingleton<ISerializer, JsonNetSerializer>();

            services.AddScoped<IPageOrchestrator<IPageContent, IDevice>, PageOrchestrator>();
            services.AddScoped<IPageOrchestrator, PageOrchestrator>();
            services.AddScoped<IDeviceAccessor<IDevice>, PageOrchestrator>();
            services.AddScoped<IPageContentAccessor<IPageContent>, PageOrchestrator>();

            services.AddBorgUserSession();

            services.AddBorgDefaultSlugifier();

            services.AddScoped<DeviceLayoutFilter>();

            services.AddScoped<IFileStorage>((s) => new FolderFileStorage(Path.Combine(env.WebRootPath, settings.Storage.Folder), s.GetService<ILoggerFactory>()));

            return services;
        }

        private static IServiceCollection AddBorgUserSession(this IServiceCollection services)
        {
            //services.AddScoped<ISessionServerResponseProvider, HttpSessionServerResponseProvider>();
            services.AddScoped<ISessionServerResponseProvider, TempDataResponseProvider>();
            services.AddScoped<IUserSession, BorgUserSession>();
            services.AddScoped<IContextAwareUserSession, BorgUserSession>();
            return services;
        }


        public static Assembly[] FireUpAssemblyScanners(this IServiceCollection services, ILoggerFactory loggerFactory, Func<Assembly, bool> predicate = null)
        {
            var depedencyAssemblyProvider = new DepedencyAssemblyProvider(loggerFactory);
            var referenceAssemblyProvider = new ReferenceAssemblyProvider(loggerFactory);

            services.AddSingleton<IAssemblyProvider>(provider => depedencyAssemblyProvider);
            services.AddSingleton<IAssemblyProvider>(provider => referenceAssemblyProvider);
            var query = depedencyAssemblyProvider.GetAssemblies().Union(referenceAssemblyProvider.GetAssemblies());
            return predicate == null ? query.Distinct().ToArray() : query.Where(predicate).Distinct().ToArray();
        }
    }
}
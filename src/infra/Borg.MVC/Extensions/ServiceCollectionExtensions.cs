using Borg.Infra;
using Borg.Infra.Storage;
using Borg.MVC;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Services.ServerResponses;
using Borg.MVC.Services.UserSession;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

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
    }
}
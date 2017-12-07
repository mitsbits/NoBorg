using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.Infra;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Timesheets.Web.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfrastructureExtensions
    {
        public static void AddInfrastructure<TSettings>(this IServiceCollection services, TSettings settings) where TSettings : WebSiteSettings
        {
            services.AddMediatR();
            services.AddScoped<ISerializer, JsonNetSerializer>();
            services.AddScoped<IPageContentAccessor<IPageContent>, PageOrchestrator>();
            services.AddScoped<IDeviceAccessor<IDevice>, PageOrchestrator>();
            services.AddScoped<IPageOrchestrator<IPageContent, IDevice>, PageOrchestrator>();
            services.AddScoped<IApplicationUserSession, UserSession>();
        }
    }
}

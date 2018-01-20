using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Borg.MVC.PlugIns.Contracts
{
    public interface IPluginDescriptor
    {
    }

    public interface IPlugInArea : IPluginDescriptor
    {
        string Area { get; }
    }

    public interface IPlugInTheme : IPluginDescriptor
    {
        string[] Themes { get; }
    }

    public interface ICanMapWhen : IPluginDescriptor
    {
        Func<HttpContext, bool> MapWhenPredicate { get; }
        Action<IApplicationBuilder, Action<IRouteBuilder>> MapWhenAction { get; }
    }

    public interface IPluginServiceRegistration : IPluginDescriptor
    {
        IServiceCollection Configure(IServiceCollection services, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment, IConfiguration Configuration);
    }
}
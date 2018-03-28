using Borg.Infra;
using Borg.Infra.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using Borg.CMS.BuildingBlocks.Contracts;

namespace Borg.MVC.PlugIns.Contracts
{
    public interface IPluginDescriptor
    {
        string Title { get; }
    }

    public interface IPlugInArea : IPluginDescriptor
    {
        string Area { get; }
        Tidings BackofficeEntryPointAction { get; }
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
        IServiceCollection Configure(IServiceCollection services, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment, IConfiguration Configuration, BorgSettings settings, Assembly[] assembliesToScan);
    }

    public interface ISecurityPlugIn : IPluginDescriptor
    {
        string[] DefinedRoles { get; }
        IDictionary<string, AuthorizationPolicy> Policies { get; }
    }

    public interface ITagHelpersPlugIn : IPluginDescriptor
    {
        string[] TagHelpers { get; }
    }
    public interface IViewComponentsPlugIn : IPluginDescriptor
    {
        string[] ViewComponents { get; }
    }

    public interface IConfigurationBlockDescriptor : IPluginDescriptor
    {
        Type[] ConfigurationBlocTypes { get; }
    }
}
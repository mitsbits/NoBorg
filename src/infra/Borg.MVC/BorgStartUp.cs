﻿using Borg.Infra;
using Borg.Infra.Services.AssemblyProvider;
using Borg.MVC.PlugIns;
using Borg.MVC.PlugIns.Contracts;
using Borg.MVC.PlugIns.Decoration;
using Borg.MVC.Services.Themes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace Borg.MVC
{
    public abstract class BorgStartUp
    {
        protected BorgStartUp(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment environment)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
            Environment = environment;
        }

        protected IConfiguration Configuration { get; }
        protected ILoggerFactory LoggerFactory { get; }

        protected IHostingEnvironment Environment { get; }

        protected BorgSettings Settings { get; set; } = new BorgSettings();

        protected IPlugInHost PlugInHost { get; set; }

        protected Assembly[] AssembliesToScan { get; set; }
        protected Assembly[] EntryPointAssemblies { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }

        protected IServiceCollection RegisterPlugins(IServiceCollection services)
        {
            AssembliesToScan = PopulateAssemblyProviders(services);
            services.TryAddSingleton(typeof(IPlugInHost), provider => PlugInHost);
            var registernmodules = PlugInHost.FilterPluginsTo<IPluginServiceRegistration>();

            foreach (var registernmodule in registernmodules)
            {
                registernmodule.Configure(services, LoggerFactory, Environment, Configuration, Settings, AssembliesToScan);
            }
            EntryPointAssemblies = ViewEngineProvidersForPluginThemes(services, AssembliesToScan);
            return services;
        }

        protected IMvcBuilder AddBorgMvc(IServiceCollection services)
        {
            var mvcbuilder = services.AddMvc();
            foreach (var entrypointassembly in EntryPointAssemblies)
            {
                mvcbuilder.AddApplicationPart(entrypointassembly);
            }

            return mvcbuilder;
        }

        protected void PopulateSettings(IServiceCollection services)
        {
            services.Config(Configuration.GetSection("Borg"), () => Settings);
        }

        protected Assembly[] PopulateAssemblyProviders(IServiceCollection services)
        {
            var assembliesToScan = services.FireUpAssemblyScanners(LoggerFactory);
            PlugInHost = new PlugInHost(LoggerFactory, assembliesToScan);
            return assembliesToScan;
        }

        protected static Assembly[] ViewEngineProvidersForPluginThemes(IServiceCollection services, Assembly[] assembliesToScan)
        {
            var entrypointassemblies = assembliesToScan.Where(x =>
                    x.GetTypes().Any(t => t.GetCustomAttributes<PlugInEntryPointControllerAttribute>() != null)).Distinct()
                .ToArray();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeViewLocationExpander());
                options.PageViewLocationFormats.Add("/Themes/Bootstrap3/Shared/{0}.cshtml");

                foreach (var entrypointassembly in entrypointassemblies)
                {
                    options.FileProviders.Add(new EmbeddedFileProvider(entrypointassembly));
                }
            });
            return entrypointassemblies;
        }
    }


}
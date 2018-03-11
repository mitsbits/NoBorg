﻿using System.Linq;
using System.Reflection;
using Borg.Cms.Basic.Lib.Features.CMS.Services;
using Borg.Infra;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.PlugIns.Contracts;
using Borg.MVC.PlugIns.Decoration;
using Borg.MVC.TagHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Presentation
{
    public sealed class PresentationPlugInDescriptor : IPluginDescriptor, IPlugInTheme, IPlugInArea, IPluginServiceRegistration, ITagHelpersPlugIn, IViewComponentslugIn
    {
        public string Title => "Presentation";

        public IServiceCollection Configure(IServiceCollection services, ILoggerFactory loggerFactory,
            IHostingEnvironment hostingEnvironment, IConfiguration Configuration, BorgSettings settings,
            Assembly[] assembliesToScan)
        {
            services.AddScoped<HtmlMetaTagHelper>();
            services.AddScoped<IComponentPageDescriptorService<int>, ComponentPageDescriptorService>();
            return services;
        }

        public string[] Themes => new[] { "Bootstrap3" };
        public string Area => Title;
        public Tidings BackofficeEntryPointAction => new Tidings();

        public string[] TagHelpers
        {
            get
            {
                var attrs = GetType().Assembly.GetTypes().Select(x => x.GetCustomAttribute<PulgInTagHelperAttribute>());
                if (!attrs.Any(x => x != null)) return new string[0];
                return attrs.Where(x => x != null).Distinct().Select(x => x.Name).ToArray();
            }
        }

        public string[] ViewComponents
        {
            get
            {
                var attrs = GetType().Assembly.GetTypes().Select(x => x.GetCustomAttribute<PulgInViewComponentAttribute>());
                if (!attrs.Any(x => x != null)) return new string[0];
                return attrs.Where(x => x != null).Distinct().Select(x => x.Name).ToArray();
            }
        }

    }


}
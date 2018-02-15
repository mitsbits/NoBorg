using System.Linq;
using System.Reflection;
using Borg.Cms.Basic.Presentation.Areas.Presentation.TagHelpers;
using Borg.Infra;
using Borg.Infra.DTO;
using Borg.MVC.PlugIns.Contracts;
using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Presentation
{
    public sealed class PresentationPlugInDescriptor : IPluginDescriptor, IPlugInTheme, IPlugInArea, IPluginServiceRegistration, ITagHelpersPlugIn
    {
        public string Title => "Presentation";

        public IServiceCollection Configure(IServiceCollection services, ILoggerFactory loggerFactory,
            IHostingEnvironment hostingEnvironment, IConfiguration Configuration, BorgSettings settings,
            Assembly[] assembliesToScan)
        {
            return services.AddScoped<HtmlMetaTagHelper>();
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
    }


}
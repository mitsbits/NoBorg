using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Borg.MVC
{
    public abstract class BorgStartUp
    {
        protected BorgStartUp(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; protected set; }
        public BorgSettings BorgSettings { get; protected set; }

        public IHostingEnvironment HostingEnvironment { get; }

        protected virtual void PopulateSettings(IServiceCollection services, string configElement = "Borg")
        {
            BorgSettings = new BorgSettings();
            services.Config(Configuration.GetSection(configElement), () => BorgSettings);
        }
    }
}
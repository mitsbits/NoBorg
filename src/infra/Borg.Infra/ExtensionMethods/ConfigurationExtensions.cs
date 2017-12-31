using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationExtensions
    {
        public static TConfig Config<TConfig>(this IServiceCollection services, IConfiguration configuration,
            Func<TConfig> pocoProvider) where TConfig : class
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (pocoProvider == null) throw new ArgumentNullException(nameof(pocoProvider));

            var config = pocoProvider();
            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }

        public static TConfig Config<TConfig>(this IServiceCollection services, IConfiguration configuration,
            TConfig config) where TConfig : class
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (config == null) throw new ArgumentNullException(nameof(config));

            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }
    }
}
using Borg.Infra.Services.Slugs;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SlugExtensions
    {
        public static IServiceCollection AddBorgDefaultSlugifier(this IServiceCollection services)
        {
            services.AddBorgInternationalCharacterToASCIIService<GreekCharacterToAsciiService>();
            services.AddSingleton<ISlugifierService, Slugifier>();
            return services;
        }

        public static IServiceCollection AddBorgInternationalCharacterToASCIIService<T>(
            this IServiceCollection services) where T : InternationalCharacterToASCIIService
        {
            services.Add(new ServiceDescriptor(typeof(InternationalCharacterToASCIIService), typeof(T),
                ServiceLifetime.Singleton));
            return services;
        }
    }
}
﻿using System;
using Borg.MVC.TagHelpers.HtmlPager;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public  static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPagination(this IServiceCollection services)
        {
            return services.AddSingleton<IPaginationSettingsProvider, NullPaginationSettingsProvider>();
        }
        public static IServiceCollection AddPagination<TSettings>(this IServiceCollection services, Func<TSettings> factory) where TSettings : IPaginationInfoStyle
        {
            return services.AddSingleton<IPaginationSettingsProvider>(c => new FactoryPaginationSettingsProvider<TSettings>(factory));
        }

        public static IServiceCollection AddPagination<TSettings>(this IServiceCollection services, IConfigurationSection config) where TSettings : class, IPaginationInfoStyle, new()
        {
            var settings = new TSettings();
            services.ConfigurePOCO(config, () => settings);
            return services.AddSingleton<IPaginationSettingsProvider>(c => new InstancePaginationSettingsProvider<TSettings>(settings));
        }
    }
}

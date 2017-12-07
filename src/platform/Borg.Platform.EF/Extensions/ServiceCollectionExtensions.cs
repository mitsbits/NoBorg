using Borg.Infra.DAL;
using Borg.Platform.EF.DAL;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    //https://forums.asp.net/t/1866727.aspx?How+do+i+get+a+collection+of+class+names+from+DbContext+
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository<TDbContext, T>(this IServiceCollection services) where TDbContext : DbContext where T : class
        {
            return AddRepository<TDbContext>(services, typeof(T));
        }

        private static IServiceCollection AddRepository<TDbContext>(IServiceCollection services, Type type) where TDbContext : DbContext
        {
            var dbType = typeof(TDbContext);
            var qryType = typeof(QueryRepository<,>);
            var instType = qryType.MakeGenericType(type, dbType);
            services.AddScoped(typeof(IQueryRepository<>).MakeGenericType(type), instType);
            return services;
        }
    }
}
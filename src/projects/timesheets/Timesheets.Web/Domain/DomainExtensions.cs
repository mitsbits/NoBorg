using Borg.Platform.EF.Contracts;
using Borg.Platform.EF.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Timesheets.Web.Domain.Services;
using Timesheets.Web.Features.Taxonomies.Services;
using Timesheets.Web.Infrastructure;

namespace Timesheets.Web.Domain
{
    public static class DomainExtensions
    {
        public static void AddDomain<TSettings>(this IServiceCollection services, TSettings settings, IHostingEnvironment env)
            where TSettings : WebSiteSettings
        {
            services.AddDbContextPool<TimesheetsDbContext>(options =>
            {
                options.UseSqlServer(settings.ConnectionStrings.TmesheetsSql);
                options.EnableSensitiveDataLogging(env.IsDevelopment());
            });

            services.AddScoped<IUnitOfWork<TimesheetsDbContext>, UnitOfWork<TimesheetsDbContext>>();

            services.AddSingleton<ITimeZoneProvider, TimeZoneProvider>();
            services.AddScoped<ITaxonomyService, TaxonomyService>();
        }

        public static void SeedDomain(this IWebHost host, string[] args)
        {
            IServiceScopeFactory services = host.Services.GetService<IServiceScopeFactory>();
            EnsureDatabase(services);
            EnsureDefaultTeams(services);
            //MakeSomeTaxonomies(services);
        }

        private static void EnsureDefaultTeams(IServiceScopeFactory services)
        {
            using (var scope = services.CreateScope())
            {
                TimesheetsDbContext dbcontext = scope.ServiceProvider.GetService<TimesheetsDbContext>();

                var ukTeam = dbcontext.Teams.FirstOrDefault(
                        x => x.Id.Equals(TeamCoutries.UK.ToString(), StringComparison.OrdinalIgnoreCase));
                if (ukTeam == null)
                {
                    ukTeam = new Team(TeamCoutries.UK, "GMT Standard Time");
                    dbcontext.Teams.Add(ukTeam);
                }

                var auzieTeam = dbcontext.Teams.FirstOrDefault(
                    x => x.Id.Equals(TeamCoutries.Australia.ToString(), StringComparison.OrdinalIgnoreCase));
                if (auzieTeam == null)
                {
                    auzieTeam = new Team(TeamCoutries.Australia, "AUS Eastern Standard Time");
                    dbcontext.Teams.Add(auzieTeam);
                }

                var greekTeam = dbcontext.Teams.FirstOrDefault(
                    x => x.Id.Equals(TeamCoutries.Greece.ToString(), StringComparison.OrdinalIgnoreCase));
                if (greekTeam == null)
                {
                    greekTeam = new Team(TeamCoutries.Greece, "GTB Standard Time");
                    dbcontext.Teams.Add(greekTeam);
                }

                dbcontext.SaveChanges();
            }
        }

        private static void EnsureDatabase(IServiceScopeFactory services)
        {
            using (var scope = services.CreateScope())
            {
                TimesheetsDbContext dbcontext = scope.ServiceProvider.GetService<TimesheetsDbContext>();
                dbcontext.Database.Migrate();
            }
        }

        //private static void MakeSomeTaxonomies(IServiceScopeFactory services)
        //{
        //    using (var scope = services.CreateScope())
        //    {
        //        TimesheetsDbContext dbcontext = scope.ServiceProvider.GetService<TimesheetsDbContext>();

        //        var p1 = new Taxonomy(null, "p1");
        //        var p2 = new Taxonomy(null, "p2");
        //        var c1 = new Taxonomy(p1.Id, "c1");
        //        var c2 = new Taxonomy(p1.Id, "c2");
        //        var c3 = new Taxonomy(p2.Id, "c3");
        //        var c4 = new Taxonomy(p2.Id, "c4");
        //        var cc1 = new Taxonomy(c1.Id, "cc1");

        //        dbcontext.Taxonomies.AddRange(new []{p1,p2,c1, c2, c3, c4, cc1});
        //        dbcontext.SaveChanges();
        //    }
        //}
    }
}
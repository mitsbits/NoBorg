using Borg.MVC;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Globalization;
using Timesheets.Web.Auth;
using Timesheets.Web.Domain;
using Timesheets.Web.Infrastructure;
using Timesheets.Web.Services.Behaviors;

namespace Timesheets.Web
{
    public class Startup : BorgStartUp
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory) : base(configuration, env, loggerFactory)
        {
            Environment = env;
        }

        private WebSiteSettings Settings { get; set; }
        private IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            BorgSettings = new BorgSettings();
            services.Config(Configuration.GetSection("Borg"), () => BorgSettings);

            Settings = new WebSiteSettings();
            services.Config(Configuration, () => Settings);
            //services.AddPagination<PaginationInfoStyle>(Configuration.GetSection("Borg:PaginationInfoStyle"));
            services.AddPagination(() => BorgSettings.PaginationInfoStyle);

            //services.AddRepository<TimesheetsDbContext, AspUser>();
            //services.AddRepository<TimesheetsDbContext, AspUserRole>();
            //services.AddRepository<TimesheetsDbContext, AspRole>();
            //services.AddRepository<TimesheetsDbContext, Worker>();

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CorrelationBehavior<,>));

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddAuth(Settings);
            services.AddDomain(Settings, Environment);
            services.AddMediatR();
            services.AddBorgFramework(HostingEnvironment, BorgSettings).AddMvc().AddFeatureFolders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715
            var options = new SessionOptions
            {
                IdleTimeout = TimeSpan.FromMinutes(10),
                Cookie =
                {
                    HttpOnly = true,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                }
            };
            app.UseSession(options);
            app.UseMvcWithDefaultRoute();

            var cultureInfo = new CultureInfo("en-GB")
            {
                NumberFormat = { CurrencySymbol = "€" }
            };

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }
    }
}
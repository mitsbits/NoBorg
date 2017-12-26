using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.MVC;
using Borg.MVC.TagHelpers.HtmlPager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Borg.Cms.Basic
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        private BorgSettings Settings { get; set; } = new BorgSettings();
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Config(Configuration.GetSection("Borg"), () => Settings);
            services.AddPagination(() => Settings.PaginationInfoStyle);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Home" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Home}/{id?}");
            });
        }
    }
}

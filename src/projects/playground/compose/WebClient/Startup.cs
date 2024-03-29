using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebClient.Hubs;

namespace WebClient
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      services.AddSignalR();

      services.AddSingleton<IHostedService, Counter>();
      //services.AddNodeServices();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseDeveloperExceptionPage();

      // For real apps, you should only use Webpack Dev Middleware at development time. For production,
      // you'll get better performance and reliability if you precompile the webpack output and simply
      // serve the resulting static files. For examples of setting up this automatic switch between
      // development-style and production-style webpack usage, see the 'templates' dir in this repo.
      app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
      {
        HotModuleReplacement = true,
        //ConfigFile = "build/webpack.base.config.js"
      });

      app.UseStaticFiles();
      app.UseSignalR(routes =>
      {
        routes.MapHub<CounterHub>("count");
      });
      app.UseMvc(routes =>
       {
         routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");

         routes.MapSpaFallbackRoute(
                  name: "spa-fallback",
                  defaults: new { controller = "Home", action = "Index" });
       });
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        public IContainer ApplicationContainer { get; private set; }
        public IHostingEnvironment Environment { get; }
        public AppSettings Settings { get; } = new AppSettings();
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Config(Configuration.GetSection("compose"), () => Settings);
            services.AddCors();
            services.AddMvc();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new AppiModule(Settings));
            builder.Populate(services);
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(
                options => options.WithOrigins("http://localhost:8801/").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()
            );
            app.UseMvc();

            var bus = ApplicationContainer.Resolve<IBusControl>();
            bus.Start();
            lifetime.ApplicationStopping.Register(() => bus.Stop());

        }
    }
}

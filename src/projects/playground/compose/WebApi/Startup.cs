using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain;
using Domain.Auth;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;

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

            var builder = new ContainerBuilder();
            builder.RegisterModule(new CommonModule(Settings));
            builder.RegisterModule(new AuthModule(services, Settings));
            builder.RegisterModule(new ApiModule(Settings));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Settings.Jwt.Issuer,
                        ValidAudience = Settings.Jwt.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Jwt.Key))
                    };
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Borg API", Version = "v1" });
            });
            services.AddBorgDefaultSlugifier();

            services.AddCors();
            services.AddMvc();

            builder.Populate(services);
            ApplicationContainer = builder.Build();

            var locator = new AutofacServiceProvider(ApplicationContainer);

            return locator;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(
                options => options.WithOrigins("http://localhost:5000/").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()
            );

            app.UseAuthentication();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();

            var bus = ApplicationContainer.Resolve<IBusControl>();
            // bus.Start();
            lifetime.ApplicationStopping.Register(() => bus.Stop());
            lifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}
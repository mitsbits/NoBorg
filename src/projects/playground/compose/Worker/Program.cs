using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Borg.Infra;
using Domain;
using Domain.Auth;
using Domain.Auth.Data;
using Domain.Messages.Contracts;
using Domain.Model.Data;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Worker
{
    class Program
    {
        private static IConfigurationRoot Configuration { get;  set; }
        private static IContainer Container { get; set; }
        private static IServiceProvider Locator { get; set; }
        private static IServiceCollection Services { get; set; } = new ServiceCollection();
        private static AppSettings Settings { get; set; } = new AppSettings();
        static void Main(string[] args)
        {
            Configuration = ConfigureSettings();
            Container = RegisterDI();
            Locator = new AutofacServiceProvider(Container);

            using (var  scope = Locator.CreateScope())
            {

                var modelseed = scope.ServiceProvider.GetService<ModelDbSeed>();
                var authseed = scope.ServiceProvider.GetService<AuthDbSeed>();

                authseed.Init().Wait(15000);
                modelseed.Init().Wait(15000);

            }

            var bus = Container.Resolve<IBusControl>();

            bus.Start();


            Console.WriteLine("Press any key to exit");
            Console.Read();

            bus.Stop();

        
        }

        private static IContainer RegisterDI()
        {

            Services.Config(Configuration.GetSection("compose"), () => Settings);
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CommonModule(Settings));
            builder.RegisterModule(new AuthModule(Services, Settings));
            builder.RegisterModule(new WorkerModule(Settings));
            builder.Populate(Services);
            return builder.Build();

        }
        private static IConfigurationRoot ConfigureSettings()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}

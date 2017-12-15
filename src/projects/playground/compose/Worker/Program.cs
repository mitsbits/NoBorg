using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain;
using Domain.Messages.Contracts;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Worker
{
    class Program
    {
        private static IConfigurationRoot Configuration { get;  set; }
        private static IContainer Container { get; set; }
        private static AppSettings Settings { get; set; } = new AppSettings();
        static void Main(string[] args)
        {
            Configuration = ConfigureSettings();
            Container = RegisterDI();

            var bus = Container.Resolve<IBusControl>();

            bus.Start();

            //bus.Publish<CreateTopic>(new
            //{
            //    CommandId = Guid.NewGuid(),
            //    Timestamp = DateTimeOffset.UtcNow,
            //    Topic = "test",
            //    UserName = "xxx"
            //});

            Console.WriteLine("Press any key to exit");
            Console.Read();

            bus.Stop();

        
        }

        private static IContainer RegisterDI()
        {
            IServiceCollection services = new ServiceCollection();
            services.Config(Configuration.GetSection("compose"), () => Settings);
            var builder = new ContainerBuilder();

            builder.RegisterModule(new WorkerModule(Settings));
            builder.RegisterModule(new CommonModule(Settings));
            builder.Populate(services);
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

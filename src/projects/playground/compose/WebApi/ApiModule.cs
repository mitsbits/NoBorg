using Autofac;
using Domain;
using MassTransit;
using System;
using Module = Autofac.Module;

namespace WebApi
{
    public class ApiModule : Module
    {
        private readonly AppSettings _settings;

        public ApiModule(AppSettings settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                {
                    var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var host = cfg.Host(new Uri(_settings.RabbitMq.Host), h =>
                        {
                            h.Username(_settings.RabbitMq.Username);
                            h.Password(_settings.RabbitMq.Password);
                        });
                    });

                    return busControl;
                })
                .As<IBusControl>()
                .As<IBus>()
                .As<IPublishEndpoint>()
                .SingleInstance();
        }
    }
}
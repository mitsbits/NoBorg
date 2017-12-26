using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using Borg.Platform.EF.DAL;
using Domain;
using Domain.Messages.Contracts;
using Domain.Model;
using MassTransit;
using MassTransit.AutofacIntegration;
using Module = Autofac.Module;

namespace Worker
{
    public class WorkerModule : Module
    {
        private readonly AppSettings _settings;
        public WorkerModule(AppSettings settings)
        {
            
            _settings = settings;
        }
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<UnitOfWork<ModelDbContext>>().As<IUnitOfWork<ModelDbContext>>();

            builder.RegisterConsumers(Assembly.GetExecutingAssembly(), typeof(CreateTopic).Assembly);
            builder.Register(context =>
                {

                    var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        
                        var host = cfg.Host(new Uri( _settings.RabbitMq.Host), h =>
                        {
                            h.Username(_settings.RabbitMq.Username);
                            h.Password(_settings.RabbitMq.Password);
                        });

                        cfg.ReceiveEndpoint("workerqueue", ec =>
                        {
                            ec.LoadFrom(context);
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Borg.Platform.EF.Contracts;
using Domain.Messages.Contracts;
using Domain.Model;
using MassTransit;

namespace Worker.Consumers
{
   public class CreateTopicHandler :IConsumer<CreateTopic>
   {
       private readonly IUnitOfWork<ModelDbContext>  _uow;

       public CreateTopicHandler(IUnitOfWork<ModelDbContext> uow)
       {
           _uow = uow;
       }

       public async Task Consume(ConsumeContext<CreateTopic> context)
       {
           var topic = new Topic()
           {
               HashTag = context.Message.Topic,
               CreateCommandId = context.Message.CommandId,
               Description = context.Message.TopicDescription,
               UserName = context.Message.UserName
           };

           var repo = _uow.ReadWriteRepo<Topic>();
           await repo.Create(topic);
           await _uow.Save();

           await context.Publish<TopicCreated>(new {Topic = topic}, context.CancellationToken);
       }
        
    }
}

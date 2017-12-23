using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Messages.Contracts;
using Domain.Model;
using MassTransit;

namespace Worker.Consumers
{
   public class CrateTopicHandler :IConsumer<CreateTopic>
   {
       private readonly ModelDbContext _db;

       public CrateTopicHandler(ModelDbContext db)
       {
           _db = db;
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
           await _db.Topics.AddAsync(topic, context.CancellationToken);
           await _db.SaveChangesAsync(context.CancellationToken);
           await context.Publish<TopicCreated>(new {Topic = topic}, context.CancellationToken);
       }
        
    }
}

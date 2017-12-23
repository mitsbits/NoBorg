using System;
using Domain.Model;

namespace Domain.Messages.Contracts
{
   public interface CreateTopic
    {
        Guid CommandId { get; }
        DateTimeOffset Timestamp { get; }
        string Topic { get; }
        string UserName { get; }
        string TopicDescription { get; }
    }

    public interface TopicCreated
    {
        ITopic Topic { get; }
    }
}

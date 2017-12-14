using System;

namespace Domain.Messages.Contracts
{
   public interface CreateTopic
    {
        Guid CommandId { get; }
        DateTimeOffset Timestamp { get; }
        string Topic { get; }
        string UserName { get; }
    }
}

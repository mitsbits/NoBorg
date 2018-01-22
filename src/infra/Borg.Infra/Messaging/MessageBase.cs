using System;

namespace Borg.Infra.Messaging
{
    public class MessageBase : ICorrelated
    {
        public Guid CorrelationId { get; } = Guid.NewGuid();
        public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
    }
}
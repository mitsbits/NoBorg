using System;

namespace Borg.Infra.Messaging
{
    public class MessageBase : ICorrelated
    {
        public DateTimeOffset Timestap = DateTimeOffset.UtcNow;
        public Guid CorrelationId { get; } = Guid.NewGuid();
    }
}
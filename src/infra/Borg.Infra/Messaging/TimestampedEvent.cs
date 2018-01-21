using System;

namespace Borg.Infra.Messaging
{
    public abstract class TimestampedEvent : MessageBase
    {
        public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
    }
}
using Borg.Infra.Messaging;
using System;

namespace Borg.Cms.Basic.Lib.Features
{
    public abstract class TimestampedEvent : MessageBase
    {
        public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
    }
}
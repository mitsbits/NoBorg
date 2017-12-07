using System;

namespace Borg.Infra.Messaging
{
    public interface ICorrelated
    {
        Guid CorrelationId { get; }
    }
}
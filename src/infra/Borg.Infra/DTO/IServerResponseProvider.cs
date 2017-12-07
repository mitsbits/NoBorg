using System.Collections.Generic;

namespace Borg.Infra.DTO
{
    public interface IServerResponseProvider<T> where T : IServerResponse
    {
        IReadOnlyCollection<T> Messages { get; }

        void Push(T message);

        T Pop();
    }
}
using System;

namespace Borg.Infra.DTO
{
    public interface IServerResponse<out TKey> : IServerResponse where TKey : IEquatable<TKey>
    {
        TKey ETag { get; }
    }

    public interface IServerResponse
    {
        ResponseStatus Status { get; set; }

        string Title { get; set; }

        string Message { get; set; }
    }
}
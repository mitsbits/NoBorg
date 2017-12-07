using System;
using Borg.MVC.BuildingBlocks;

namespace Borg.MVC.Services.UserSession
{
    public interface IUserSession : ICanContextualize
    {
        string SessionId { get; }
        DateTimeOffset SessionStart { get; }
        string UserIdentifier { get; }
        string UserName { get; }
        bool IsAuthenticated();
        T Setting<T>(string key, T value);
        T Setting<T>(string key);
    }
}

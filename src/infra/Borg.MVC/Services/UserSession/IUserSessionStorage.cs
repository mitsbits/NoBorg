using Borg.Infra.Storage.Contracts;

namespace Borg.MVC.Services.UserSession
{
    public interface IUserSessionStorage : IFileStorage
    {
        IScopedFileStorage UserStorage { get; }
    }
}
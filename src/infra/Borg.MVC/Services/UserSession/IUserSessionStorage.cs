using Borg.Infra.Storage;

namespace Borg.MVC.Services.UserSession
{
    public interface IUserSessionStorage : IFileStorage
    {
        IScopedFileStorage UserStorage { get; }
    }
}
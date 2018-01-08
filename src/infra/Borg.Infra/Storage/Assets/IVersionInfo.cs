using Borg.Infra.Storage.Contracts;

namespace Borg.Infra.Storage.Assets.Contracts
{
    public interface IVersionInfo
    {
        int Version { get; }

        IFileSpec FileSpec { get; }
    }
}
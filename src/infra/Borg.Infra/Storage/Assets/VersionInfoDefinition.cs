using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;

namespace Borg.Infra.Storage.Assets
{
    public class VersionInfoDefinition : IVersionInfo
    {
        public VersionInfoDefinition(int version, IFileSpec fileSpec)
        {
            Version = version;
            FileSpec = fileSpec;
        }

        public int Version { get; }
        public IFileSpec FileSpec { get; }
    }
}
using Borg.Infra.DDD.Contracts;
using Borg.Infra.Storage.Assets.Contracts;
using System.Collections.Generic;

namespace Borg.Platform.EF.Assets
{
    public class AssetRecord : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<VersionRecord> Versions { get; set; } = new HashSet<VersionRecord>();

        public DocumentState DocumentState { get; set; }
        public int CurrentVersion { get; set; } = 0;
    }
}
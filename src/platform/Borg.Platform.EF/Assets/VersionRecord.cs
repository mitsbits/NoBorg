using Borg.Infra.DDD;

namespace Borg.Platform.EF.Assets
{
    public class VersionRecord : IEntity<int>
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int AssetRecordId { get; set; }
        public virtual AssetRecord AssetRecord { get; set; }
        public int FileRecordId { get; set; }
        public virtual FileRecord FileRecord { get; set; }
    }
}
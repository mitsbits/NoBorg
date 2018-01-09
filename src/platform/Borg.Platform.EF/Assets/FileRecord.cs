using System;
using Borg.Infra.DDD;

namespace Borg.Platform.EF.Assets
{
    public class FileRecord : IEntity<int>
    {
        public int Id { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastWrite { get; set; }
        public DateTime? LastRead { get; set; }
        public long SizeInBytes { get; set; }
        public string MimeType { get; set; }
        public virtual VersionRecord VersionRecord { get; set; }
    }
}
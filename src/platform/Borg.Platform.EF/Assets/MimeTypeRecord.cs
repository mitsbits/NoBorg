using System.Collections.Generic;

namespace Borg.Platform.EF.Assets
{
    public class MimeTypeRecord
    {
        public string Extension { get; set; }
        public string MimeType { get; set; }
        internal virtual ICollection<FileRecord> Files { get; set; }
    }
}
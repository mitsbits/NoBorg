using Borg.Infra.Storage.Contracts;

namespace Borg.Infra.Storage
{
    public class MimeTypeSpec : IMimeTypeSpec
    {
        public MimeTypeSpec(string extension, string mimeType)
        {
            Extension = extension;
            MimeType = mimeType;
        }

        public string Extension { get; }
        public string MimeType { get; }
    }
}
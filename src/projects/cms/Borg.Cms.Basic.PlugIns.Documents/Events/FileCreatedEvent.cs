using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.PlugIns.Documents.Events
{
    public class FileCreatedEvent : MessageBase, INotification
    {
        public FileCreatedEvent(int fileId, string mimeType)
        {
            FileId = fileId;
            MimeType = mimeType;
        }

        public int FileId { get; }
        public string MimeType { get; }
    }
}
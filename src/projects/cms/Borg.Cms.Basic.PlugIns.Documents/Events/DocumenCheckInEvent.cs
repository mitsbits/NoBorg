using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.PlugIns.Documents.Events
{
    public class DocumenCheckInEvent : MessageBase, INotification
    {
        public DocumenCheckInEvent(int documentId, string userHandle, int checkInVersion)
        {
            DocumentId = documentId;
            UserHandle = userHandle;
            CheckInVersion = checkInVersion;
        }

        public string UserHandle { get; }
        public int DocumentId { get; }
        public int CheckInVersion { get; }
    }
}
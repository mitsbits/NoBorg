using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.PlugIns.Documents.Events
{
    public class DocumenCheckOutEvent : MessageBase, INotification
    {
        public DocumenCheckOutEvent(int documentId, string userHandle, int checkOutVersion)
        {
            DocumentId = documentId;
            UserHandle = userHandle;
            CheckOutVersion = checkOutVersion;
        }

        public string UserHandle { get; }
        public int DocumentId { get; }
        public int CheckOutVersion { get; }
    }
}
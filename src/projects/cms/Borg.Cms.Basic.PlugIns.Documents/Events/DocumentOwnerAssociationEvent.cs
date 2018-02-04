using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.PlugIns.Documents.Events
{
    public class DocumentOwnerAssociationEvent : MessageBase, INotification
    {
        public DocumentOwnerAssociationEvent(int documentId, string userHandle, DocumentOwnerAssociationOperation associationOperation)
        {
            DocumentId = documentId;
            UserHandle = userHandle;
            AssociationOperation = associationOperation;
        }

        public DocumentOwnerAssociationOperation AssociationOperation { get; }
        public string UserHandle { get; }
        public int DocumentId { get; }
    }
}
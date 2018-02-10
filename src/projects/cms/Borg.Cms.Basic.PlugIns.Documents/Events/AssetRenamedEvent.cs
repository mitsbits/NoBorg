using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.PlugIns.Documents.Events
{
    public class AssetRenamedEvent : MessageBase, INotification
    {
        public AssetRenamedEvent(int documentId, string newName)
        {
            DocumentId = documentId;
            NewName = newName;
        }

        public string NewName { get; }
        public int DocumentId { get; }
    }
}
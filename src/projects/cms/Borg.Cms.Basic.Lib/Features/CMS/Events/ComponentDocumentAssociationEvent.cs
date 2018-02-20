using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.CMS.Events
{
    internal class ComponentDocumentAssociationEvent : MessageBase, INotification
    {
        public ComponentDocumentAssociationEvent(int componentId, int documentId)
        {
            ComponentId = componentId;
            DocumentId = documentId;
        }

        public int ComponentId { get; }
        public int DocumentId { get; }

    }
}
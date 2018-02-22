using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.CMS.Events
{
    internal class ComponentDocumentDisassociationEvent : MessageBase, INotification
    {
        public ComponentDocumentDisassociationEvent(int componentId, int documentId)
        {
            ComponentId = componentId;
            DocumentId = documentId;
        }

        public int ComponentId { get; }
        public int DocumentId { get; }
    }
}
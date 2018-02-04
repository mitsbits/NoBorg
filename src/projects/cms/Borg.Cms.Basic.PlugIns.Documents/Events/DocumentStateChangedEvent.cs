using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.PlugIns.Documents.Events
{
    public abstract class DocumentStateChangedEvent : MessageBase, INotification
    {
        protected DocumentStateChangedEvent(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
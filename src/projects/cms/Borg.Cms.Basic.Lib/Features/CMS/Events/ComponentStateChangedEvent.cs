using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.CMS.Events
{
    public abstract class ComponentStateChangedEvent : MessageBase, INotification
    {
        protected ComponentStateChangedEvent(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
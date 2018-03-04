using Borg.Infra.DAL;
using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.CMS.Categories.Events
{
    public class CategoryGroupingStateChangedEvent : MessageBase, INotification
    {
        public CategoryGroupingStateChangedEvent(int recordId, CRUDOperation operation)
        {
            RecordId = recordId;
            Operation = operation;
        }

        public int RecordId { get; }
        public CRUDOperation Operation { get; }
    }


    public class CategoryStateChangedEvent : MessageBase, INotification
    {
        public CategoryStateChangedEvent(int recordId, CRUDOperation operation)
        {
            RecordId = recordId;
            Operation = operation;
        }

        public int RecordId { get; }
        public CRUDOperation Operation { get; }
    }
}
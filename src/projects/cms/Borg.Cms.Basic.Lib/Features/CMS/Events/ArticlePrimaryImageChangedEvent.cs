using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.CMS.Events
{
    public class ArticlePrimaryImageChangedEvent : MessageBase, INotification
    {
        public ArticlePrimaryImageChangedEvent(int recordId, (int documentId, int fileId) current, (int? documentId, int? fileId) prev)
        {
            RecordId = recordId;
            Current = current;
            Previous = prev;
        }

        public int RecordId { get; }
        public (int documentId, int fileId) Current { get; }
        public (int? documentId, int? fileId) Previous { get; }
    }
}
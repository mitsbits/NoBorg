using System;
using System.Collections.Generic;
using System.Text;
using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.CMS.Events
{

    class ArticlePrimaryImageChangedEvent : MessageBase, INotification
    {
        public ArticlePrimaryImageChangedEvent(int recordId, (int documentId, int fileId) current, (int? documentId, int? fileId) prev)
        {
            RecordId = recordId;
            Current = current;
            Previous = prev;
        }
        public int RecordId { get; }
        private (int documentId, int fileId) Current;
        private (int? documentId, int? fileId) Previous;


    }
}

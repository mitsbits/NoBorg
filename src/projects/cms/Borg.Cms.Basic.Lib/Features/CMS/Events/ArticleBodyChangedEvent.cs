using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.CMS.Events
{
    public class ArticleBodyChangedEvent : MessageBase, INotification
    {
        public ArticleBodyChangedEvent(int articleId, string currentBody, string previousBody)
        {
            ArticleId = articleId;
            CurrentBody = currentBody;
            PrevioustBody = previousBody;
        }

        public int ArticleId { get; }
        public string PrevioustBody { get; }
        public string CurrentBody { get; }
    }
}
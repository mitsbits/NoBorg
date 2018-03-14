using Borg.CMS.Components.Contracts;
using Borg.Infra.Messaging;
using MediatR;
using System.Collections.Generic;

namespace Borg.Cms.Basic.Lib.Features.CMS.Events
{
    public class ArticleHtmlMetasChangedEvent : MessageBase, INotification
    {
        public ArticleHtmlMetasChangedEvent(int articleId, IEnumerable<IHtmlMeta> metas)
        {
            ArticleId = articleId;
            Metas = metas;
        }

        public int ArticleId { get; }
        public IEnumerable<IHtmlMeta> Metas { get; }
    }
}
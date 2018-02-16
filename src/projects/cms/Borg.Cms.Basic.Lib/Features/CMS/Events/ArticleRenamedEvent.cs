using Borg.Infra.Messaging;
using Borg.MVC.BuildingBlocks.Contracts;
using MediatR;
using System.Collections.Generic;

namespace Borg.Cms.Basic.Lib.Features.CMS.Events
{
    public class ArticleRenamedEvent : MessageBase, INotification
    {
        public ArticleRenamedEvent(int id, string oldTitle, string currentTitle, string oldSlug, string currentSlug)
        {
            Id = id;
            OldTitle = oldTitle;
            CurrentTitle = currentTitle;
            OldSlug = oldSlug;
            CurrentSlug = currentSlug;
        }

        public int Id { get; }
        public string OldTitle { get; }
        public string CurrentTitle { get; }
        public string OldSlug { get; }
        public string CurrentSlug { get; }
    }

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
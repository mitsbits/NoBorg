﻿using System.Collections.Generic;
using Borg.Infra.Messaging;
using Borg.MVC.BuildingBlocks.Contracts;
using MediatR;

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
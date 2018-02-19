using Borg.Infra.Messaging;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Borg.Cms.Basic.Lib.Features.CMS.Events
{
    public class ArticleHtmlMetasEvent : MessageBase, INotification
    {
        public ArticleHtmlMetasEvent(int id, string metas)
        {
            Id = id;
            Metas = JsonConvert.DeserializeObject<HtmlMeta[]>(metas);
        }

        public int Id { get; }
        public IHtmlMeta[] Metas { get; }
    }
}
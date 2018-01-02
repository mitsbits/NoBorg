using Borg.Infra.DDD;
using System;

namespace Borg.Cms.Basic.Lib.Features.Content
{
    public class ContentItemRecord : IPublishable, IEntity<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public bool IsPublished { get; set; }
        public string Author { get; set; }
        public DateTimeOffset PublisheOn { get; set; }

        public void Publish()
        {
            IsPublished = true;
        }

        public void Suspend()
        {
            IsPublished = false;
        }
    }
}
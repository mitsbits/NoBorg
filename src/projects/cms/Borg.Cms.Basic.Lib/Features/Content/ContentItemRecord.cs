﻿using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.CMS;
using System;

namespace Borg.Cms.Basic.Lib.Features.Content
{
    public class ContentItemRecord : IEntity<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public DateTimeOffset PublisheDate { get; set; }
        public DateTimeOffset? LastRevisionDate { get; set; }
        public virtual NavigationItemState NavigationItemState { get; set; }
    }
}
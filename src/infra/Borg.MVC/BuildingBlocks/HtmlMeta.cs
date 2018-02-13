﻿using Borg.MVC.BuildingBlocks.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Borg.MVC.BuildingBlocks
{
    public class HtmlMeta : IHtmlMeta
    {
        [JsonProperty]
        public virtual string Content
        {
            get;
            set;
        }

        [JsonProperty]
        public virtual string HttpEquiv
        {
            get;
            set;
        }

        [JsonProperty]
        public virtual string Name
        {
            get;
            set;
        }

        [JsonProperty]
        public virtual string Scheme
        {
            get;
            set;
        }

        [JsonIgnore]
        public virtual bool ShouldRender => !string.IsNullOrWhiteSpace(Content) && (!string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(HttpEquiv));
    }

    [JsonArray(false)]
    public class HtmlMetaSet : List<HtmlMeta>
    {
    }
}
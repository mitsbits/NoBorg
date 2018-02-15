using System;
using Borg.MVC.BuildingBlocks.Contracts;
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
        [JsonIgnore]
        public virtual bool IsHttpEquiv => !string.IsNullOrWhiteSpace(HttpEquiv);
        [JsonIgnore]
        public virtual bool IsOpenGraph => !string.IsNullOrWhiteSpace(Name) && Name.StartsWith("og:", StringComparison.CurrentCultureIgnoreCase);
        [JsonIgnore]
        public virtual bool IsTwitterCard => !string.IsNullOrWhiteSpace(Name)  && Name.StartsWith("twitter:", StringComparison.CurrentCultureIgnoreCase);
        [JsonIgnore]
        public virtual bool IsBasic => TypeIdentifier == "BASIC";
        [JsonIgnore]
        public virtual string TypeIdentifier
        {
            get
            {
                if (IsOpenGraph) return "OPENGRAPH";
                if (IsTwitterCard) return "TWITTERCARD";
                return IsHttpEquiv ? "HTTPEQUIV" : "BASIC";
            }
        }
    }

    [JsonArray(false)]
    public class HtmlMetaSet : List<HtmlMeta>
    {
    }
}
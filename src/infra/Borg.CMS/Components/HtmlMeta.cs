using System;
using Borg.CMS.Components.Contracts;
using Newtonsoft.Json;

namespace Borg.CMS.Components
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
        public virtual bool IsTwitterCard => !string.IsNullOrWhiteSpace(Name) && Name.StartsWith("twitter:", StringComparison.CurrentCultureIgnoreCase);

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
}
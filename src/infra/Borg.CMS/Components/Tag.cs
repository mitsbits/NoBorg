using Borg.CMS.Components.Contracts;
using Newtonsoft.Json;

namespace Borg.CMS.Components
{
    public class Tag : ITag
    {
        public Tag() { }
        public Tag(string tagDisplay, string tagSlug):this()
        {
            TagDisplay = tagDisplay;
            TagSlug = tagSlug;
        }
        [JsonProperty]
        public string TagDisplay { get; set; }
        [JsonProperty]
        public string TagSlug { get; set; }
    }
}
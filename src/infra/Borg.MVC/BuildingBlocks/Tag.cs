using Borg.MVC.BuildingBlocks.Contracts;
using Newtonsoft.Json;

namespace Borg.MVC.BuildingBlocks
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
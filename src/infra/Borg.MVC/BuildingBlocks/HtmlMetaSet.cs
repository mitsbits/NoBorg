using System.Collections.Generic;
using Newtonsoft.Json;

namespace Borg.MVC.BuildingBlocks
{
    [JsonArray(false)]
    public class HtmlMetaSet : List<HtmlMeta>
    {
    }
}
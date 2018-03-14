using System.Collections.Generic;
using Newtonsoft.Json;

namespace Borg.CMS.Components
{
    [JsonArray(false)]
    public class HtmlMetaSet : List<HtmlMeta>
    {
    }
}
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Borg.CMS.Components
{
    [JsonArray(false)]
    public class HtmlMetaSet : List<HtmlMeta>
    {
    }
}
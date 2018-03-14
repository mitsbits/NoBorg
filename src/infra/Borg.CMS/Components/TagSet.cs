using System.Collections.Generic;
using Newtonsoft.Json;

namespace Borg.CMS.Components
{
    [JsonArray(false)]
    public class TagSet : List<Tag>
    {
        
    }
}
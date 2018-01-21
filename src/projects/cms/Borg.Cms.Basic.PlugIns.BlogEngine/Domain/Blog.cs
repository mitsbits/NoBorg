using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.Infra.DDD;

namespace Borg.Cms.Basic.PlugIns.BlogEngine.Domain
{
    public class Blog : IEntity<int>
    {
        public int Id { get; protected set; }

    }

    public class Blogger : IEntity<int>
    {
        public int Id { get;  protected set; }
    }


    public class Blogpost : IEntity<int>
    {
        public int Id { get;protected set; }
    }
}

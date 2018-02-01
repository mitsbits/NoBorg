using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.Infra.DDD.Contracts;

namespace Borg.Cms.Basic.PlugIns.Documents.Data
{
    public class DocumentState : IEntity<int>
    {
        public int Id { get; }
    }
}

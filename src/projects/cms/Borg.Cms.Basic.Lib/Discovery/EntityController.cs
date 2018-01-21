using System;
using System.Collections.Generic;
using System.Text;
using Borg.Infra.DDD;
using Borg.MVC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Lib.Discovery
{
   public class EntityController<Tentity> : BorgController where Tentity : IEntity
    {
        public EntityController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }
}

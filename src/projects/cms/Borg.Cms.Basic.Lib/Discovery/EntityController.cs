using Borg.Infra.DDD.Contracts;
using Borg.MVC;
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
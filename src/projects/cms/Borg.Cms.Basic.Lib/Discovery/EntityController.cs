using Borg.Infra.DDD.Contracts;
using Borg.MVC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Lib.Discovery
{
    public class EntityController<TEntity> : BorgController where TEntity : IEntity
    {
        public EntityController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public IActionResult Index()
        {
            var t = 5;
            return Ok(t);
        }
    }
}
using Borg.Infra.DDD.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.MVC.Contrrollers
{
    public abstract class EntityController<TEntity> : BorgController where TEntity : IEntity
    {
        protected EntityController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

    }

}
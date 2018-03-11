using System.Linq;
using System.Threading.Tasks;
using Borg.Infra.DAL;
using Borg.Infra.DDD.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Borg.Platform.EF
{
   public abstract class StaticDatabaseEntityStore<TEntity> : StaticEntityStore<TEntity> where TEntity : IEntity 
   {
       protected StaticDatabaseEntityStore(ILoggerFactory loggerFactory) : base(loggerFactory)
       {
       }

       public abstract Task Populate(IQueryable<TEntity> dbSet);

   }
}

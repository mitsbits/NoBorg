using System.Threading.Tasks;

namespace Borg.Infra.DDD.Contracts
{
    public interface IEntityFactory
    {
        Task<TEntity> Build<TEntity>() where TEntity : IEntity;
    }
}
namespace Borg.Infra.DAL
{
    public interface IReadWriteRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : class
    {
    }
}
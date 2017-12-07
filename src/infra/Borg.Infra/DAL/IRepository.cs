namespace Borg.Infra.DAL
{
    public interface IRepository //marker
    {
    }

    public interface IRepository<T> : IRepository //marker
    {
    }
}
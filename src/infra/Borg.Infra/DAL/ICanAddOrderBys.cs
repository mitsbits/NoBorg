namespace Borg.Infra.DAL
{
    public interface ICanAddOrderBys<T> where T : class
    {
        ICanAddAndBuildOrderBys<T> Add(OrderByInfo<T> item);
    }
}
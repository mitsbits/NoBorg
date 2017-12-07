namespace Borg.Infra.DAL
{
    public interface ICanAddAndBuildOrderBys<T> : ICanAddOrderBys<T>, ICanProduceOrderBys<T> where T : class
    {
    }
}
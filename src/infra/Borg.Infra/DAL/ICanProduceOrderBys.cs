using System.Collections.Generic;

namespace Borg.Infra.DAL
{
    public interface ICanProduceOrderBys<T> where T : class
    {
        IEnumerable<OrderByInfo<T>> Build();
    }
}
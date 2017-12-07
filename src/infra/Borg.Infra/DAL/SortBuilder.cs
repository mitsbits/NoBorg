using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Borg.Infra.DAL
{
    public class SortBuilder<T> : SortBuilder, ICanAddAndBuildOrderBys<T> where T : class
    {
        internal SortBuilder()
        {
        }

        ICanAddAndBuildOrderBys<T> ICanAddOrderBys<T>.Add(OrderByInfo<T> item)
        {
            _derectives.Add(item.Directive);
            return this;
        }

        IEnumerable<OrderByInfo<T>> ICanProduceOrderBys<T>.Build()
        {
            return _derectives.Select(x => new OrderByInfo<T>(x));
        }
    }

    public abstract class SortBuilder
    {
        protected readonly List<string> _derectives = new List<string>();

        public static ICanAddAndBuildOrderBys<T> Get<T>() where T : class
        {
            return new SortBuilder<T>();
        }

        public static ICanAddAndBuildOrderBys<T> Get<T>(Expression<Func<T, dynamic>> property, bool ascending = true)
            where T : class
        {
            return new SortBuilder<T>().Add(property, ascending);
        }
    }
}
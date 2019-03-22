using System;
using System.Linq.Expressions;

namespace Cqrs.Common.Queries.Sorting
{
    public static class OrderedFetchStrategyExtenstions
    {
        public static IOrderedFetchStrategy<T> SortBy<T>(this IOrderedFetchStrategy<T> source, Expression<Func<T, object>> path)
        {
            var orderCreteria = new OrderCreteria<T>(path, OrderDirection.ASC);
            source.SortBy(orderCreteria);
            return source;
        }

        public static IOrderedFetchStrategy<T> SortByDescending<T>(this IOrderedFetchStrategy<T> source, Expression<Func<T, object>> path)
        {
            var orderCreteria = new OrderCreteria<T>(path, OrderDirection.DESC);
            source.SortBy(orderCreteria);
            return source;
        }
    }
}

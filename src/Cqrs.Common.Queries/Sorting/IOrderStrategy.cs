using System.Collections.Generic;

namespace Cqrs.Common.Queries.Sorting
{
    public interface IOrderedFetchStrategy<T> : IFetchStrategy<T>
    {
        IEnumerable<OrderCreteria<T>> Ordering { get; }

        IOrderedFetchStrategy<T> SortBy(OrderCreteria<T> creteria);
    }
}

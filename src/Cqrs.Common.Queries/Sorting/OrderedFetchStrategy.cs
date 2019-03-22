using System.Collections.Generic;

namespace Cqrs.Common.Queries.Sorting
{
    public abstract class OrderedFetchStrategy<T> : FetchStrategy<T>, IOrderedFetchStrategy<T>
    {
        private readonly IList<OrderCreteria<T>> _creterias = new List<OrderCreteria<T>>();

        public IEnumerable<OrderCreteria<T>> Ordering => _creterias;

        public IOrderedFetchStrategy<T> SortBy(OrderCreteria<T> creteria)
        {
            if (!_creterias.Contains(creteria))
            {
                _creterias.Add(creteria);
            }

            return this;
        }
    }
}

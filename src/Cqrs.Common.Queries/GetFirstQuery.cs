namespace Cqrs.Common.Queries
{
    #region Using
    using Cqrs.Common.Queries.FetchStateries;
    using Cqrs.Common.Queries.Sorting;
    using Cqrs.Core.Abstractions;
    using NSpecifications;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    #endregion

    public class GetFirstQuery<T> : IQuery<T>
    {
        public GetFirstQuery(ISpecification<T> specification)
            : this(specification, fetchStrategy: new FetchAllStatery<T>(), orderCreteria: null)
        {
        }

        public GetFirstQuery(ISpecification<T> specification, string orderBy)
            : this(specification, new FetchAllStatery<T>(), new OrderCreteria<T>(orderBy, OrderDirection.ASC))
        {               
        }

        public GetFirstQuery(ISpecification<T> specification, Expression<Func<T, object>> orderBy)
            : this(specification, new FetchAllStatery<T>(), new OrderCreteria<T>(orderBy, OrderDirection.ASC))
        {
        }

        public GetFirstQuery(ISpecification<T> specification, IFetchStrategy<T> fetchStrategy)
            : this(specification, fetchStrategy, null)
        {
        }

        public GetFirstQuery(ISpecification<T> specification, IFetchStrategy<T> fetchStrategy, OrderCreteria<T> orderCreteria)
        {
            Specification = specification;
            FetchStrategy = fetchStrategy;
            if (orderCreteria != null)
            {
                Sorting = new OrderCreteria<T>[] { orderCreteria };
            }
        }

        public ISpecification<T> Specification { get; }

        public IFetchStrategy<T> FetchStrategy { get; }

        public IEnumerable<OrderCreteria<T>> Sorting { get; }
    }
}

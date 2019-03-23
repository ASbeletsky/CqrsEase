using Cqrs.Common.Queries.Sorting;
using Cqrs.Core.Abstractions;
using NSpecifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Cqrs.Common.Queries
{
    public class FindFirstQuery<T, TSpecification> : IQuery<T>
        where TSpecification : ISpecification<T>
    {
        public FindFirstQuery(TSpecification specification)
            : this(specification, null)
        {

        }

        public FindFirstQuery(TSpecification specification, IFetchStrategy<T> fetchStrategy)
            : this(specification, fetchStrategy, null)
        {

        }

        public FindFirstQuery(TSpecification specification, IFetchStrategy<T> fetchStrategy, Expression<Func<T, object>> sortKeySelector)
        {
            Specification = specification;
            FetchStrategy = fetchStrategy;
            if (sortKeySelector != null)
            {
                Sorting = new OrderCreteria<T>[] { new OrderCreteria<T>(sortKeySelector, OrderDirection.ASC) };
            }
        }

        public TSpecification Specification { get; }

        public IFetchStrategy<T> FetchStrategy { get; }

        public IEnumerable<OrderCreteria<T>> Sorting { get; }
    }
}

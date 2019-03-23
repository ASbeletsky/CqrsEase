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
            : this(specification, fetchStrategy: new FetchAllStatery<T>())
        {

        }

        public GetFirstQuery(ISpecification<T> specification, IFetchStrategy<T> fetchStrategy)
            : this(specification, fetchStrategy, null)
        {

        }

        public GetFirstQuery(ISpecification<T> specification, IFetchStrategy<T> fetchStrategy, Expression<Func<T, object>> sortKeySelector)
        {
            Specification = specification;
            FetchStrategy = fetchStrategy;
            if (sortKeySelector != null)
            {
                Sorting = new OrderCreteria<T>[] { new OrderCreteria<T>(sortKeySelector, OrderDirection.ASC) };
            }
        }

        public ISpecification<T> Specification { get; }

        public IFetchStrategy<T> FetchStrategy { get; }

        public IEnumerable<OrderCreteria<T>> Sorting { get; }
    }
}

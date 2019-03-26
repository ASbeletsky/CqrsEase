namespace Cqrs.Common.Queries
{
    #region Using
    using Cqrs.Common.Queries.FetchStateries;
    using Cqrs.Common.Queries.Sorting;
    using Cqrs.Core.Abstractions;
    using NSpecifications;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    #endregion

    public class GetFirstQuery<T> : IQuery<T>
    {
        public GetFirstQuery(ISpecification<T> specification)
            : this(specification, fetchStrategy: null)
        {
        }

        public GetFirstQuery(params Expression<Func<T, object>>[] orderBy)
            : this (specification: null, orderBy: orderBy)
        {
        }

        public GetFirstQuery(IFetchStrategy<T> fetchStrategy, params Expression<Func<T, object>>[] orderBy)
            : this(specification: null, fetchStrategy: fetchStrategy, orderBy: orderBy.Select(sortKey => new OrderCreteria<T>(sortKey, OrderDirection.ASC)).ToArray())
        {
        }

        public GetFirstQuery(ISpecification<T> specification, params string[] orderBy)
            : this(specification, null, orderBy.Select(sortKey => new OrderCreteria<T>(sortKey, OrderDirection.ASC)).ToArray())
        {               
        }

        public GetFirstQuery(ISpecification<T> specification, params Expression<Func<T, object>>[] orderBy)
            : this(specification, null, orderBy.Select(sortKey => new OrderCreteria<T>(sortKey, OrderDirection.ASC)).ToArray())
        {
        }

        public GetFirstQuery(ISpecification<T> specification, IFetchStrategy<T> fetchStrategy)
            : this(specification, fetchStrategy, null)
        {
        }

        public GetFirstQuery(ISpecification<T> specification, IFetchStrategy<T> fetchStrategy, params OrderCreteria<T>[] orderBy)
        {
            Specification = specification;
            FetchStrategy = fetchStrategy;
            Sorting = orderBy;
        }

        public ISpecification<T> Specification { get; }

        public IFetchStrategy<T> FetchStrategy { get; }

        public IEnumerable<OrderCreteria<T>> Sorting { get; }
    }
}

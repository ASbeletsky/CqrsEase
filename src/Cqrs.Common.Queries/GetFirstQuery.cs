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
            : this(specification, orderBy: null)
        {

        }

        public GetFirstQuery(ISpecification<T> specification, string orderBy)
            : this(specification, new FetchAllStatery<T>(), orderBy)
        {
                
        }

        public GetFirstQuery(ISpecification<T> specification, IFetchStrategy<T> fetchStrategy)
            : this(specification, fetchStrategy, null)
        {

        }

        public GetFirstQuery(ISpecification<T> specification, IFetchStrategy<T> fetchStrategy, string orderBy)
        {
            Specification = specification;
            FetchStrategy = fetchStrategy;
            if (orderBy != null)
            {
                Sorting = new OrderCreteria<T>[] { new OrderCreteria<T>(orderBy, OrderDirection.ASC) };
            }
        }

        public ISpecification<T> Specification { get; }

        public IFetchStrategy<T> FetchStrategy { get; }

        public IEnumerable<OrderCreteria<T>> Sorting { get; }
    }
}

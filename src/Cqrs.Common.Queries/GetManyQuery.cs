namespace Cqrs.Common.Queries
{
    #region Using
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Common.Queries.ProjectionStateries;
    using Cqrs.Common.Queries.Sorting;
    using Cqrs.Core.Abstractions;
    using NSpecifications;
    using System.Collections.Generic;
    #endregion

    public class GetManyQuery<T> : IQuery<IEnumerable<T>>, IQuery<ILimitedEnumerable<T>>
    {

        #region Get all use cases

        public GetManyQuery()
            : this(sortingParams: null)

        {
        }

        public GetManyQuery(params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: new FetchAllStatery<T>(), sortingParams: sortingParams)
        {
        }

        public GetManyQuery(FetchStrategy<T> fetchStrategy, params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: fetchStrategy, specification: null, pagination: null, sorting: sortingParams)

        {
        }

        #endregion

        #region Filter use cases

        public GetManyQuery(ISpecification<T> specification)
            : this(specification: specification, sortingParams: null)
        {
        }

        public GetManyQuery(ISpecification<T> specification, params OrderCreteria<T>[] sortingParams)
            : this(specification: specification, pagination: null, sortingParams: sortingParams)
        {
        }

        public GetManyQuery(ISpecification<T> specification, IPage pagination, params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: new FetchAllStatery<T>(), specification: specification, pagination: pagination, sorting: sortingParams)

        {
        }

        public GetManyQuery(IFetchStrategy<T> fetchStrategy, ISpecification<T> specification, params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: fetchStrategy, specification: specification, pagination: null, sorting: sortingParams)

        {
        }

        #endregion

        #region Paging use cases

        public GetManyQuery(IPage pagination)
            : this(pagination: pagination, sortingParams: null)
        {
        }

        public GetManyQuery(IPage pagination, params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: new FetchAllStatery<T>(), pagination: pagination, sortingParams: sortingParams)
        {
        }

        public GetManyQuery(FetchStrategy<T> fetchStrategy, IPage pagination, params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: fetchStrategy, specification: null, pagination: pagination, sorting: sortingParams)
        {
        }

        #endregion

        public GetManyQuery(IFetchStrategy<T> fetchStrategy, ISpecification<T> specification, IPage pagination, IEnumerable<OrderCreteria<T>> sorting)
        {
            Specification = specification;
            FetchStrategy = fetchStrategy;
            Sorting = sorting;
            Pagination = pagination;
        }

        public IFetchStrategy<T> FetchStrategy { get; }
        public ISpecification<T> Specification { get; }
        public IEnumerable<OrderCreteria<T>> Sorting { get; }
        public IPage Pagination { get; }
    }
}

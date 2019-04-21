namespace Cqrs.Common.Queries
{
    using System.Collections.Generic;
    #region Using
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Common.Queries.Sorting;
    using NSpecifications;
    #endregion

    public class ProjectManyQuery<TSource, TDest> : GetManyQuery<TDest>
    {
        #region Project all use cases

        public ProjectManyQuery(params OrderCreteria<TDest>[] sortingParams) : base(sortingParams)
        {
        }

        public ProjectManyQuery(FetchStrategy<TDest> fetchStrategy, params OrderCreteria<TDest>[] sortingParams) : base(fetchStrategy, sortingParams)
        {
        }

        #endregion

        #region Filter uses cases

        public ProjectManyQuery(ISpecification<TDest> specification) : base(specification)
        {
        }

        public ProjectManyQuery(ISpecification<TDest> specification, params OrderCreteria<TDest>[] sortingParams) : base(specification, sortingParams)
        {
        }

        public ProjectManyQuery(ISpecification<TDest> specification, IPage pagination, params OrderCreteria<TDest>[] sortingParams) : base(specification, pagination, sortingParams)
        {
        }

        public ProjectManyQuery(IFetchStrategy<TDest> fetchStrategy, ISpecification<TDest> specification, params OrderCreteria<TDest>[] sortingParams) : base(fetchStrategy, specification, sortingParams)
        {
        }

        #endregion

        #region Paging use cases

        public ProjectManyQuery(IPage pagination) : base(pagination)
        {
        }

        public ProjectManyQuery(IPage pagination, params OrderCreteria<TDest>[] sortingParams) : base(pagination, sortingParams)
        {
        }

        public ProjectManyQuery(IFetchStrategy<TDest> fetchStrategy, IPage pagination, params OrderCreteria<TDest>[] sortingParams) : base(fetchStrategy, pagination, sortingParams)
        {
        }

        #endregion

        public ProjectManyQuery(IFetchStrategy<TDest> fetchStrategy, ISpecification<TDest> specification, IPage pagination, IEnumerable<OrderCreteria<TDest>> sorting) : base(fetchStrategy, specification, pagination, sorting)
        {
        }
    }
}

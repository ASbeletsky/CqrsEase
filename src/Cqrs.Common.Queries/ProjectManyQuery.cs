namespace Cqrs.Common.Queries
{
    #region Using
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Common.Queries.Sorting;
    using NSpecifications;
    #endregion

    public class ProjectManyQuery<TSource, TDest> : GetManyQuery<TDest>
    {
        #region Project all use cases

        public ProjectManyQuery()
        {

        }

        public ProjectManyQuery(params OrderCreteria<TDest>[] sortingParams) : base(sortingParams)
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

        #endregion

        #region Paging use cases

        public ProjectManyQuery(IPage pagination) : base(pagination)
        {
        }

        public ProjectManyQuery(IPage pagination, params OrderCreteria<TDest>[] sortingParams) : base(pagination, sortingParams)
        {
        }

        #endregion
    }
}

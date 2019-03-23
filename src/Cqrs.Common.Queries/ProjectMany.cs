namespace Cqrs.Common.Queries
{
    #region Using
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Common.Queries.Sorting;
    using NSpecifications;
    #endregion

    public class ProjectMany<TSource, TDest> : GetManyQuery<TDest>
    {
        #region Project all use cases

        public ProjectMany()
        {

        }

        public ProjectMany(params OrderCreteria<TDest>[] sortingParams) : base(sortingParams)
        {
        }

        #endregion

        #region Filter uses cases

        public ProjectMany(ISpecification<TDest> specification) : base(specification)
        {
        }

        public ProjectMany(ISpecification<TDest> specification, params OrderCreteria<TDest>[] sortingParams) : base(specification, sortingParams)
        {
        }

        public ProjectMany(ISpecification<TDest> specification, IPage pagination, params OrderCreteria<TDest>[] sortingParams) : base(specification, pagination, sortingParams)
        {
        }

        #endregion

        #region Paging use cases

        public ProjectMany(IPage pagination) : base(pagination)
        {
        }

        public ProjectMany(IPage pagination, params OrderCreteria<TDest>[] sortingParams) : base(pagination, sortingParams)
        {
        }

        #endregion
    }
}

namespace Cqrs.Common.Queries
{
    #region Using
    using NSpecifications;
    using Cqrs.Common.Queries.Sorting;
    #endregion

    public class ProjectFirstQuery<TSource, TDest> : GetFirstQuery<TDest>
    {
        public ProjectFirstQuery(ISpecification<TDest> specification) : base(specification)
        {
        }

        public ProjectFirstQuery(ISpecification<TDest> specification, string orderBy) : base(specification, null, new OrderCreteria<TDest>(orderBy, OrderDirection.ASC))
        {
        }
    }
}

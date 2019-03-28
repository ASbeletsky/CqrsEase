namespace Cqrs.Common.Queries
{
    #region Using
    using NSpecifications;
    using Cqrs.Common.Queries.Sorting;
    using System;
    using System.Linq.Expressions;
    #endregion

    public class ProjectFirstQuery<TSource, TDest> : GetFirstQuery<TDest>
    {
        public ProjectFirstQuery(ISpecification<TDest> specification) : base(specification)
        {
        }

        public ProjectFirstQuery(params Expression<Func<TDest, object>>[] orderBy) : base(orderBy)
        {
        }

        public ProjectFirstQuery(IFetchStrategy<TDest> fetchStrategy, params Expression<Func<TDest, object>>[] orderBy) : base(fetchStrategy, orderBy)
        {
        }

        public ProjectFirstQuery(ISpecification<TDest> specification, params string[] orderBy) : base(specification, orderBy)
        {
        }

        public ProjectFirstQuery(ISpecification<TDest> specification, params Expression<Func<TDest, object>>[] orderBy) : base(specification, orderBy)
        {
        }

        public ProjectFirstQuery(ISpecification<TDest> specification, IFetchStrategy<TDest> fetchStrategy) : base(specification, fetchStrategy)
        {
        }

        public ProjectFirstQuery(ISpecification<TDest> specification, IFetchStrategy<TDest> fetchStrategy, params OrderCreteria<TDest>[] orderBy) : base(specification, fetchStrategy, orderBy)
        {
        }
    }
}

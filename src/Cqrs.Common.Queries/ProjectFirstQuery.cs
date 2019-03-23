namespace Cqrs.Common.Queries
{
    #region Using
    using NSpecifications;
    using System;
    using System.Linq.Expressions;
    #endregion

    public class ProjectFirstQuery<TSource, TDest> : GetFirstQuery<TDest>
    {
        public ProjectFirstQuery(ISpecification<TDest> specification) : base(specification)
        {
        }

        public ProjectFirstQuery(ISpecification<TDest> specification, Expression<Func<TDest, object>> sortKeySelector) : base(specification, null, sortKeySelector)
        {
        }
    }
}

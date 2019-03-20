namespace Cqrs.Common.Queries.ProjectionStateries
{
    #region Using
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    #endregion

    public class ProjectOnlyStatery<T> : ProjectionStatery<T>
    {
        public ProjectOnlyStatery(params string[] includedPaths) : base(includedPaths)
        {
        }

        public ProjectOnlyStatery(params Expression<Func<T, object>>[] includedPaths)
            : base(includedPaths.Select(e => e.ToPropertyName()))
        {
        }
    }
}

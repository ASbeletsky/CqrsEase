namespace Cqrs.Common.Queries.FetchStateries
{
    #region Using
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    #endregion

    public class FetchOnlyStratery<T> : FetchStrategy<T>
    {
        public FetchOnlyStratery(params string[] includedPaths) : base(includedPaths)
        {
        }

        public FetchOnlyStratery(params Expression<Func<T, object>>[] includedPaths)
            : base(includedPaths.Select(e => e.ToPropertyName()))
        {
        }
    }
}

namespace Cqrs.Common.Queries.FetchStrategies
{
    #region Using
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    #endregion

    public class FetchOnlyStrategy<T> : FetchStrategy<T>
    {
        public FetchOnlyStrategy(params string[] includedPaths) : base(includedPaths)
        {
        }

        public FetchOnlyStrategy(params Expression<Func<T, object>>[] includedPaths)
            : base(includedPaths.Select(e => e.ToPropertyName()))
        {
        }
    }
}

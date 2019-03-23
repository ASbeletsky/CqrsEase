namespace Cqrs.Common.Queries.FetchStateries
{
    #region Using
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    #endregion

    public class FetchOnlyStatery<T> : FetchStrategy<T>
    {
        public FetchOnlyStatery(params string[] includedPaths) : base(includedPaths)
        {
        }

        public FetchOnlyStatery(params Expression<Func<T, object>>[] includedPaths)
            : base(includedPaths.Select(e => e.ToPropertyName()))
        {
        }
    }
}

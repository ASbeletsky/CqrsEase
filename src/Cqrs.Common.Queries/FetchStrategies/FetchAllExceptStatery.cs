namespace Cqrs.Common.Queries.FetchStateries
{
    #region Using
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    #endregion

    public class FetchAllExceptStatery<T> : FetchStrategy<T>
    {
        public FetchAllExceptStatery(params string[] excludedPaths)
            : base(typeof(T).GetProperiesNames().Except(excludedPaths))
        {
                
        }

        public FetchAllExceptStatery(params Expression<Func<T, object>>[] excludedPaths)
            : this(excludedPaths.Select(e => e.ToPropertyName()).ToArray())
        {
                
        }
    }
}

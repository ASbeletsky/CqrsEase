namespace Cqrs.Common.Queries.FetchStrategies
{
    #region Using
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    #endregion

    public class FetchAllExceptStrategy<T> : FetchStrategy<T>
    {
        public FetchAllExceptStrategy(params string[] excludedPaths)
            : base(typeof(T).GetProperiesNames().Except(excludedPaths))
        {
                
        }

        public FetchAllExceptStrategy(params Expression<Func<T, object>>[] excludedPaths)
            : this(excludedPaths.Select(e => e.ToPropertyName()).ToArray())
        {
                
        }
    }
}

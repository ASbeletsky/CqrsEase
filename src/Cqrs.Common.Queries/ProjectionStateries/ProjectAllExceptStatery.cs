namespace Cqrs.Common.Queries.ProjectionStateries
{
    #region Using
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    #endregion

    public class ProjectAllExceptStatery<T> : ProjectionStatery<T>
    {
        public ProjectAllExceptStatery(params string[] excludedPaths)
            : base(typeof(T).GetProperiesNames().Except(excludedPaths))
        {
                
        }

        public ProjectAllExceptStatery(params Expression<Func<T, object>>[] excludedPaths)
            : this(excludedPaths.Select(e => e.ToPropertyName()).ToArray())
        {
                
        }
    }
}

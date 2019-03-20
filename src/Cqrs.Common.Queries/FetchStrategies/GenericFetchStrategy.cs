namespace Cqrs.Common.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class GenericFetchStrategy<T> : FetchStrategy<T>
    {
        public GenericFetchStrategy(Expression<Func<T, object>> path) 
        {
            Include(path.ToPropertyName());
        }
    }

    internal class NestedFetchStrategy<T, TPreviousResult> : FetchStrategy<T>, INestedFetchStrategy<T, TPreviousResult>
    {
        public NestedFetchStrategy(IEnumerable<string> includePaths)
        {
            foreach(var path in includePaths)
            {
                Include(path);
            }
        }
    }
}
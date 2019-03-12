namespace Cqrs.Common.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IFetchStrategy<T>
    {
        IEnumerable<string> IncludePaths { get; }       
        INestedFetchStrategy<T, TResult> Include<TResult>(Expression<Func<T, TResult>> path);
    }

    public interface INestedFetchStrategy<T, TPreviousResult> : IFetchStrategy<T>
    {
        INestedFetchStrategy<T, TResult> ThenInclude<TResult>(Expression<Func<TPreviousResult, TResult>> path);
    }
}
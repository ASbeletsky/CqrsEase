namespace Cqrs.Common.Queries
{
    using System;
    using System.Linq.Expressions;

    public static class FetchStrategyExtenstions
    {
        public static INestedFetchStrategy<T, TResult> Include<T, TResult>(this IFetchStrategy<T> source, Expression<Func<T, TResult>> path)
        {
            source.Include(path.ToPropertyName());
            return new NestedFetchStrategy<T, TResult>(source.IncludedPaths);
        }

        public static INestedFetchStrategy<T, TResult> ThenInclude<T, TPreviousResult, TResult>(this INestedFetchStrategy<T, TPreviousResult> source, Expression<Func<TPreviousResult, TResult>> path)
        {
            source.Include(path.ToPropertyName());
            return new NestedFetchStrategy<T, TResult>(source.IncludedPaths);
        }
    }
}
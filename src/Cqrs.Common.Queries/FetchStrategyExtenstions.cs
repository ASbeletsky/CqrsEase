namespace Cqrs.Common.Queries
{
    using System;
    using System.Linq.Expressions;

    public static class FetchStrategyExtenstions
    {
        public static INestedFetchStrategy<T, TResult> Include<T, TResult>(this IFetchStrategy<T> source, Expression<Func<T, TResult>> path)
        {
            throw new NotSupportedException();
        }

        public static INestedFetchStrategy<T, TResult> ThenInclude<T, TPreviousResult, TResult>(this INestedFetchStrategy<T, TPreviousResult> source, Expression<Func<TPreviousResult, TResult>> path)
        {
            throw new NotSupportedException();
        }
    }
}
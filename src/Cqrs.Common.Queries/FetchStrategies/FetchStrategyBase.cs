namespace Cqrs.Common.Queries
{
    using System;
    using System.Linq.Expressions;

    public abstract class FetchStrategyBase<T> : FetchStrategy<T>
    {
        protected FetchStrategyBase()
        {
            Include(Selector.ToPropertyName());
        }

        public abstract Expression<Func<T, object>> Selector { get; }
    }
}
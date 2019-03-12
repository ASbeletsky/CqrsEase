namespace Cqrs.Common.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public abstract class FetchStrategyBase<T> : IFetchStrategy<T>
    {
        private readonly IList<string> _properties = new List<string>();

        public IEnumerable<string> IncludePaths => _properties;

        public FetchStrategyBase()
        {
            var path = Selector();
            _properties.Add(path.ToPropertyName());
        }

        public abstract Expression<Func<T, object>> Selector();
    }
}
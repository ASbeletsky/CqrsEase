namespace Cqrs.Common.Queries
{
    using System.Collections.Generic;

    public interface IFetchStrategy<T>
    {
        IEnumerable<string> IncludePaths { get; }
    }

    public interface INestedFetchStrategy<T, TPreviousResult> : IFetchStrategy<T>
    {
        
    }
}
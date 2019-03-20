namespace Cqrs.Common.Queries
{
    using System.Collections.Generic;

    public interface IFetchStrategy<T>
    {
        IEnumerable<string> IncludedPaths { get; }

        void Include(string path);
    }

    public interface INestedFetchStrategy<T, TPreviousResult> : IFetchStrategy<T>
    {
        
    }
}
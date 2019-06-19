namespace CqrsEase.Common.Queries.FetchStrategies
{
    using System.Collections.Generic;

    public interface IFetchStrategy<T>
    {
        IEnumerable<string> FetchedPaths { get; }
        void Include(string path);
        void IncludeRange(IEnumerable<string> paths);
        INestedFetchStrategy<T, TFetchedMember> Include<TFetchedMember>(string path) where TFetchedMember : class;
    }

    public interface INestedFetchStrategy<T, TPreviousMember> : IFetchStrategy<T>
        where TPreviousMember : class
    {
        
    }
}
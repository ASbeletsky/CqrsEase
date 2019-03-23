namespace Cqrs.Common.Queries
{
    using System.Collections.Generic;

    public interface IFetchStrategy<T>
    {
        IEnumerable<string> FetchedPaths { get; }
        void Include(string path);
        INestedFetchStrategy<T, TFetchedMember> Include<TFetchedMember>(string path) where TFetchedMember : class;
    }

    public interface INestedFetchStrategy<T, TPreviousMember> : IFetchStrategy<T>
        where TPreviousMember : class
    {
        
    }
}
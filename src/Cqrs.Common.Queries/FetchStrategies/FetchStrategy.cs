namespace Cqrs.Common.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class FetchStrategy<T> : IFetchStrategy<T>
    {
        protected readonly IList<string> _fetchedPaths = new List<string>();

        public FetchStrategy(IEnumerable<string> fetchedPaths)
        {
            _fetchedPaths = fetchedPaths.ToList();
        }

        public IEnumerable<string> FetchedPaths => _fetchedPaths;

        public void Include(string path)
        {
            if(!_fetchedPaths.Contains(path))
            {
                _fetchedPaths.Add(path);
            }
        }

        public INestedFetchStrategy<T, TFetchedMember> Include<TFetchedMember>(string path) where TFetchedMember : class
        {
            Include(path);
            return new NestedFetchStrategy<T, TFetchedMember>(_fetchedPaths);
        }

        public void IncludeRange(IEnumerable<string> paths)
        {
            foreach(var path in paths)
            {
                Include(path);
            }
        }
    }

    internal class NestedFetchStrategy<T, TPreviousMember> : FetchStrategy<T>, INestedFetchStrategy<T, TPreviousMember>
        where TPreviousMember : class
    {
        public NestedFetchStrategy(IEnumerable<string> fetchedPaths) : base(fetchedPaths)
        {
        }
    }
}
namespace Cqrs.Common.Queries
{
    using System.Collections.Generic;

    public abstract class FetchStrategy<T> : IFetchStrategy<T>
    {
        private readonly IList<string> _properties = new List<string>();

        public IEnumerable<string> IncludedPaths => _properties;

        public void Include(string path)
        {
            if(!_properties.Contains(path))
            {
                _properties.Add(path);
            }  
        }
    }
}
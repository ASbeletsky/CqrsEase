namespace Cqrs.Common.Queries
{
    public class GenericFetchStrategy<T, TResult> : IFetchStrategy<T>, INestedFetchStrategy<T, TResult>
    {
        private readonly IList<string> properties;

        public GenericFetchStrategy()
        {
            properties = new List<string>();
        }

        public IEnumerable<string> IncludePaths { get; private set; }

        public IFetchStrategy<T, TResult> Include(Expression<Func<T, TResult>> path)
        {
            properties.Add(path.ToPropertyName());
            return this;
        }

        public IFetchStrategy<T, TResult> Include(string path)
        {
            properties.Add(path);
            return this;
        }
    }
}
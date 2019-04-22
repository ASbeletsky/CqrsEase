namespace Cqrs.Common.Queries.FetchStrategies
{
    public class FetchAllStrategy<T> : FetchStrategy<T>
    {
        public FetchAllStrategy() : base(typeof(T).GetProperiesNames())
        {
        }
    }
}

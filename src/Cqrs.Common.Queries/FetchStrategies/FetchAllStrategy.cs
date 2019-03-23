namespace Cqrs.Common.Queries.FetchStateries
{
    using System.Collections.Generic;

    public class FetchAllStatery<T> : FetchStrategy<T>
    {
        public FetchAllStatery() : base(typeof(T).GetProperiesNames())
        {
        }
    }
}

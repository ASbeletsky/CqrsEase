namespace Cqrs.Common.Queries.ProjectionStateries
{
    using System.Collections.Generic;

    public class FetchAllStatery<T> : FetchStrategy<T>
    {
        public FetchAllStatery() : base(typeof(T).GetProperiesNames())
        {
        }
    }
}

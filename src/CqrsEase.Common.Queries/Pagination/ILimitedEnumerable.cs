using System.Collections.Generic;

namespace CqrsEase.Common.Queries.Pagination
{
    public interface ILimitedEnumerable<out T> : IEnumerable<T>
    {
        int TotalCount { get; }
    }
}

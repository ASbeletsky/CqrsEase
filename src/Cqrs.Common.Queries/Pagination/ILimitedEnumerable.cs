using System.Collections.Generic;

namespace Cqrs.Common.Queries.Pagination
{
    public interface ILimitedEnumerable<out T> : IEnumerable<T>
    {
        int TotalCount { get; }
    }
}

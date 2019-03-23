using System;
using System.Collections.Generic;
using System.Text;

namespace Cqrs.Common.Queries
{
    public interface ILimitedEnumerable<out T> : IEnumerable<T>
    {
        int TotalCount { get; }
    }
}

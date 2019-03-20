namespace Cqrs.Common.Queries.ProjectionStateries
{
    using System.Collections.Generic;

    public interface IProjectionStatery<T>
    {
        IEnumerable<string> IncludedPaths { get; }
    }
}

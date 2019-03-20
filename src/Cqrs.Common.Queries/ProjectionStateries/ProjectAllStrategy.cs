namespace Cqrs.Common.Queries.ProjectionStateries
{
    using System.Collections.Generic;

    public class ProjectionStatery<T> : IProjectionStatery<T>
    {
        public ProjectionStatery(IEnumerable<string> includedPaths)
        {
            IncludedPaths = includedPaths;
        }

        public IEnumerable<string> IncludedPaths { get; }

        public static ProjectionStatery<T> ProjectAll => new ProjectionStatery<T>(typeof(T).GetProperiesNames());
    }
}

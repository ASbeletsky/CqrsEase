namespace Cqrs.Common.Queries
{
    #region
    using Cqrs.Common.Queries.ProjectionStateries;
    using Cqrs.Core.Abstractions;
    using Cqrs.Model.Abstractions;
    #endregion

    public class ProjectByIdQuery<TKey, TEntity, TDto> : IQuery<TDto>
            where TEntity : class, Identifiable<TKey>
    {
        public TKey Id { get; private set; }

        public IFetchStrategy<TEntity> FetchStrategy { get; set; }

        public IProjectionStatery<TDto> ProjectionStrategy { get; set; }

        public ProjectByIdQuery(TKey id)
            : this(id, ProjectionStatery<TDto>.ProjectAll)
        {
        }

        public ProjectByIdQuery(TKey id, IProjectionStatery<TDto> projectionStatery)
            : this(id, null, projectionStatery)
        {
        }

        public ProjectByIdQuery(TKey id, IFetchStrategy<TEntity> fetchStrategy, IProjectionStatery<TDto> projectionStatery)
        {
            Id = id;
            FetchStrategy = fetchStrategy;
            ProjectionStrategy = projectionStatery;
        }
    }
}

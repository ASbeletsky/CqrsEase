namespace Cqrs.Common.Queries
{
    #region
    using Cqrs.Common.Queries.FetchStateries;
    using Cqrs.Core.Abstractions;
    using Cqrs.Model.Abstractions;
    #endregion

    public class ProjectByIdQuery<TKey, TEntity, TDto> : IQuery<TDto>
            where TEntity : class, Identifiable<TKey>
    {
        public TKey Id { get; private set; }

        public IFetchStrategy<TDto> FetchStrategy { get; set; }

        public ProjectByIdQuery(TKey id)
            : this(id, new FetchAllStatery<TDto>())
        {
        }

        public ProjectByIdQuery(TKey id, IFetchStrategy<TDto> projectionStatery)
            : this(id, null, projectionStatery)
        {
        }

        public ProjectByIdQuery(TKey id, IFetchStrategy<TEntity> fetchStrategy, IFetchStrategy<TDto> projectionStatery)
        {
            Id = id;
            FetchStrategy = projectionStatery;
        }
    }
}

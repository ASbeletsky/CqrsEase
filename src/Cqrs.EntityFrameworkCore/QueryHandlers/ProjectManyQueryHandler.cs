namespace Cqrs.EntityFrameworkCore.QueryHandlers
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore.DataSource;
    using NSpecifications;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    #endregion

    public class ProjectManyQueryHandler<TSource, TDest>
        : GetManyQueryHandler<TDest>
        , IQueryHandler<ProjectManyQuery<TSource, TDest>, IEnumerable<TDest>>
        , IQueryHandler<ProjectManyQuery<TSource, TDest>, ILimitedEnumerable<TDest>>
        , IQueryHandlerAsync<ProjectManyQuery<TSource, TDest>, IEnumerable<TDest>>
        , IQueryHandlerAsync<ProjectManyQuery<TSource, TDest>, ILimitedEnumerable<TDest>>
        where TSource : class
        where TDest : class
    {
        public IProjector Projector { get; }

        public ProjectManyQueryHandler(EfDataSourceBased dataSource, IProjector projector) : base(dataSource)
        {
            Projector = projector;
        }

        public ProjectManyQueryHandler(DataSourceFactory dataSourceFactory, IProjector projector)
            : this(dataSourceFactory.GetForEntity<TSource>(), projector)
        {
        }

        protected override IQueryable<TDest> GetSourceCollection()
        {
            return Projector.ProjectTo<TDest>(DataSource.Query<TSource>());
        }

        public IEnumerable<TDest> Request(ProjectManyQuery<TSource, TDest> query)
        {
            return base.Request(query);
        }

        public async Task<IEnumerable<TDest>> RequestAsync(ProjectManyQuery<TSource, TDest> query)
        {
            return await base.RequestAsync(query);
        }

        ILimitedEnumerable<TDest> IQueryHandler<ProjectManyQuery<TSource, TDest>, ILimitedEnumerable<TDest>>.Request(ProjectManyQuery<TSource, TDest> query)
        {
            return (this as IQueryHandler<GetManyQuery<TDest>, ILimitedEnumerable<TDest>>).Request(query);
        }

        async Task<ILimitedEnumerable<TDest>> IQueryHandlerAsync<ProjectManyQuery<TSource, TDest>, ILimitedEnumerable<TDest>>.RequestAsync(ProjectManyQuery<TSource, TDest> query)
        {
            return await (this as IQueryHandlerAsync<GetManyQuery<TDest>, ILimitedEnumerable<TDest>>).RequestAsync(query);
        }
    }
}

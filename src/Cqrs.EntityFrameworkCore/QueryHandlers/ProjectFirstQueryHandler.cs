namespace Cqrs.EntityFrameworkCore.QueryHandlers
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.FetchStateries;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore.DataSource;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    #endregion

    public class ProjectFirstQueryHandler<TSource, TDest>
        : GetFirstQueryHandler<TDest>
        , IQueryHandler<ProjectFirstQuery<TSource, TDest>, TDest>
        , IQueryHandlerAsync<ProjectFirstQuery<TSource, TDest>, TDest>
        where TSource : class
        where TDest : class
    {
        public ProjectFirstQueryHandler(EfDataSourceBased dataSource, IProjector projector)
            : base(dataSource)
        {
            Projector = projector;
        }

        public ProjectFirstQueryHandler(DataSourceFactory dataSourceFactory, IProjector projector)
            : this(dataSourceFactory.GetForEntity<TSource>(), projector)
        {
        }

        public IProjector Projector { get; }

        protected override IQueryable<TDest> GetSourceCollection(GetFirstQuery<TDest> query)
        {
            return Projector.ProjectTo<TDest>(DataSource.Query<TSource>());
        }

        public TDest Request(ProjectFirstQuery<TSource, TDest> query)
        {
            return base.Request(query);
        }

        public async Task<TDest> RequestAsync(ProjectFirstQuery<TSource, TDest> query)
        {
            return await base.RequestAsync(query);
        }
    }
}

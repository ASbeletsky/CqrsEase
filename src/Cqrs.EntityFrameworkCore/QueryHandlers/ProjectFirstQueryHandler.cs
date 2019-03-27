namespace Cqrs.EntityFrameworkCore.QueryHandlers
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore.DataSource;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    #endregion

    public class ProjectFirstQueryHandler<TSource, TDest>
        : IQueryHandler<ProjectFirstQuery<TSource, TDest>, TDest>
        , IQueryHandlerAsync<ProjectFirstQuery<TSource, TDest>, TDest>
        where TSource : class
        where TDest : class
    {
        public ProjectFirstQueryHandler(EfDataSourceBased dataSource, IProjector projector)
        {
            DataSource = dataSource;
            Projector = projector;
        }

        public ProjectFirstQueryHandler(DataSourceFactory dataSourceFactory, IProjector projector)
            : this(dataSourceFactory.GetForEntity<TSource>(), projector)
        {
        }

        public EfDataSourceBased DataSource { get; }
        public IProjector Projector { get; }

        protected IQueryable<TDest> PrepareQuery(ProjectFirstQuery<TSource, TDest> query)
        {
            return Projector.ProjectTo<TDest>(DataSource.Query<TSource>()).MaybeWhere(query.Specification).MaybeSort(query.Sorting).ApplyFetchStrategy(query.FetchStrategy, DataSource._dbContext);
        }

        public TDest Request(ProjectFirstQuery<TSource, TDest> query)
        {
            return PrepareQuery(query).FirstOrDefault();
        }

        public async Task<TDest> RequestAsync(ProjectFirstQuery<TSource, TDest> query)
        {
            return await PrepareQuery(query).FirstOrDefaultAsync();
        }
    }
}

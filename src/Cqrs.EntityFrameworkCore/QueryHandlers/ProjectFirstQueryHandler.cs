namespace Cqrs.EntityFrameworkCore.QueryHandlers
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.FetchStrategies;
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
        public ProjectFirstQueryHandler(EfDataSourceBased dataSource, IProjector projector, IFetchStrategy<TSource> defaultFetchStrategy, IFetchStrategy<TDest> defaultDestFetchStrategy)
        {
            DataSource = dataSource;
            Projector = projector;
            DefaultSourceFetchStrategy = defaultFetchStrategy;
            DefaultDestFetchStrategy = defaultDestFetchStrategy;
        }

        public ProjectFirstQueryHandler(DataSourceFactory dataSourceFactory, IProjector projector, IFetchStrategy<TSource> defaultFetchStrategy, IFetchStrategy<TDest> defaultDestFetchStrategy)
            : this(dataSourceFactory.GetForEntity<TSource>(), projector, defaultFetchStrategy, defaultDestFetchStrategy)
        {
        }

        public EfDataSourceBased DataSource { get; }
        public IProjector Projector { get; }
        public IFetchStrategy<TSource> DefaultSourceFetchStrategy { get; private set; }
        public IFetchStrategy<TDest> DefaultDestFetchStrategy { get; }

        protected IQueryable<TDest> PrepareQuery(ProjectFirstQuery<TSource, TDest> query)
        {
            var destinationFetchStrategy = query.FetchStrategy ?? DefaultDestFetchStrategy;
            var source = DataSource.Query<TSource>().ApplyFetchStrategy(DefaultSourceFetchStrategy, DataSource._dbContext);        
            return Projector.ProjectTo<TSource, TDest>(source, destinationFetchStrategy.FetchedPaths)
                .MaybeWhere(query.Specification)
                .MaybeSort(query.Sorting)
                .ApplyFetchStrategy(destinationFetchStrategy);
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

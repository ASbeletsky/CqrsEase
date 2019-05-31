namespace Cqrs.EntityFrameworkCore.QueryHandlers
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.FetchStrategies;
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore.DataSource;
    using Microsoft.EntityFrameworkCore;
    using NSpecifications;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    #endregion

    public class ProjectManyQueryHandler<TSource, TDest>
        : IQueryHandler<ProjectManyQuery<TSource, TDest>, IEnumerable<TDest>>
        , IQueryHandler<ProjectManyQuery<TSource, TDest>, ILimitedEnumerable<TDest>>
        , IQueryHandlerAsync<ProjectManyQuery<TSource, TDest>, IEnumerable<TDest>>
        , IQueryHandlerAsync<ProjectManyQuery<TSource, TDest>, ILimitedEnumerable<TDest>>
        where TSource : class
        where TDest : class
    {
        public EfDataSourceBased DataSource { get; }
        public IProjector Projector { get; }

        public ProjectManyQueryHandler(EfDataSourceBased dataSource, IProjector projector)
        {
            DataSource = dataSource;
            Projector = projector;
        }

        public ProjectManyQueryHandler(DataSourceFactory dataSourceFactory, IProjector projector)
            : this(dataSourceFactory.GetForEntity<TSource>(), projector)
        {
        }


        protected IQueryable<TDest> GetFilteredSourceCollection(ISpecification<TDest> specification)
        {
            var fetchAllSourceDataStrategy = new FetchOnlyStrategy<TSource>(typeof(TSource).GetProperiesNames().ToArray());
            var source = DataSource.Query<TSource>().ApplyFetchStrategy(fetchAllSourceDataStrategy, DataSource._dbContext);
            return Projector.ProjectTo<TDest>(source).MaybeWhere(specification);
        }

        protected IQueryable<TDest> PrepareDataQuery(GetManyQuery<TDest> query)
        {
            return GetFilteredSourceCollection(query.Specification)
                .MaybeSort(query.Sorting)
                .MaybeTake(query.Pagination)
                .ApplyFetchStrategy(query.FetchStrategy, DataSource._dbContext);
        }

        public IEnumerable<TDest> Request(ProjectManyQuery<TSource, TDest> query)
        {
            return PrepareDataQuery(query).ToList();
        }

        public async Task<IEnumerable<TDest>> RequestAsync(ProjectManyQuery<TSource, TDest> query)
        {
            return await PrepareDataQuery(query).ToListAsync();
        }

        ILimitedEnumerable<TDest> IQueryHandler<ProjectManyQuery<TSource, TDest>, ILimitedEnumerable<TDest>>.Request(ProjectManyQuery<TSource, TDest> query)
        {
            var count = GetFilteredSourceCollection(query.Specification).Count();
            IEnumerable<TDest> data = count > 0 ? Request(query) : Enumerable.Empty<TDest>();
            return new LimitedEnumerable<TDest>(data, count);
        }

        async Task<ILimitedEnumerable<TDest>> IQueryHandlerAsync<ProjectManyQuery<TSource, TDest>, ILimitedEnumerable<TDest>>.RequestAsync(ProjectManyQuery<TSource, TDest> query)
        {
            var count = await GetFilteredSourceCollection(query.Specification).CountAsync();
            IEnumerable<TDest> data = count > 0 ? await RequestAsync(query) : Enumerable.Empty<TDest>();
            return new LimitedEnumerable<TDest>(data, count);
        }
    }
}

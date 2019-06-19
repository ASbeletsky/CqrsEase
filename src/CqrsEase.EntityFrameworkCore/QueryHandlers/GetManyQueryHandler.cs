namespace CqrsEase.EntityFrameworkCore.QueryHandlers
{
    #region Using
    using CqrsEase.Common.Queries;
    using CqrsEase.Common.Queries.FetchStrategies;
    using CqrsEase.Common.Queries.Pagination;
    using CqrsEase.Core.Abstractions;
    using CqrsEase.EntityFrameworkCore.DataSource;
    using Microsoft.EntityFrameworkCore;
    using NSpecifications;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    #endregion

    public class GetManyQueryHandler<T>
        : IQueryHandler<GetManyQuery<T>, IEnumerable<T>>
        , IQueryHandler<GetManyQuery<T>, ILimitedEnumerable<T>>
        , IQueryHandlerAsync<GetManyQuery<T>, IEnumerable<T>>
        , IQueryHandlerAsync<GetManyQuery<T>, ILimitedEnumerable<T>>
        where T : class
    {
        public GetManyQueryHandler(EfDataSourceBased dataSource, IFetchStrategy<T> defaultFetchStrategy)
        {
            DataSource = dataSource;
            DefaultFetchStrategy = defaultFetchStrategy;
        }

        public GetManyQueryHandler(DataSourceFactory dataSourceFactory, IFetchStrategy<T> defaultFetchStrategy)
            : this(dataSourceFactory.GetForEntity<T>(), defaultFetchStrategy)
        {
        }

        public EfDataSourceBased DataSource { get; }
        public IFetchStrategy<T> DefaultFetchStrategy { get; }

        protected IQueryable<T> GetFilteredSourceCollection(ISpecification<T> specification)
        {
            return DataSource.Query<T>().MaybeWhere(specification);
        }

        protected IQueryable<T> PrepareDataQuery(GetManyQuery<T> query)
        {
            return GetFilteredSourceCollection(query.Specification)
                .MaybeSort(query.Sorting)
                .MaybeTake(query.Pagination)
                .ApplyFetchStrategy(query.FetchStrategy ?? DefaultFetchStrategy, DataSource._dbContext);
        }

        public IEnumerable<T> Request(GetManyQuery<T> query)
        {
            return PrepareDataQuery(query).ToList();
        }

        public async Task<IEnumerable<T>> RequestAsync(GetManyQuery<T> query)
        {
            return await PrepareDataQuery(query).ToListAsync();
        }

        ILimitedEnumerable<T> IQueryHandler<GetManyQuery<T>, ILimitedEnumerable<T>>.Request(GetManyQuery<T> query)
        {
            var count = GetFilteredSourceCollection(query.Specification).Count();
            IEnumerable<T> data = count > 0 ? Request(query) : Enumerable.Empty<T>();
            return new LimitedEnumerable<T>(data, count);
        }

        async Task<ILimitedEnumerable<T>> IQueryHandlerAsync<GetManyQuery<T>, ILimitedEnumerable<T>>.RequestAsync(GetManyQuery<T> query)
        {
            var count = await GetFilteredSourceCollection(query.Specification).CountAsync();
            IEnumerable<T> data = count > 0 ? await RequestAsync(query) : Enumerable.Empty<T>();
            return new LimitedEnumerable<T>(data, count);
        }
    }
}

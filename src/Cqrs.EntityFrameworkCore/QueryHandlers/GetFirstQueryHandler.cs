namespace Cqrs.EntityFrameworkCore.QueryHandlers
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore.DataSource;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using static Cqrs.EntityFrameworkCore.CqrsExtensions;
    #endregion

    public class GetFirstQueryHandler<TEntity> 
        : IQueryHandler<GetFirstQuery<TEntity>, TEntity>
        , IQueryHandlerAsync<GetFirstQuery<TEntity>, TEntity>
        where TEntity : class
    {
        public GetFirstQueryHandler(EfDataSourceBased dataSource, IFetchStrategy<TEntity> defaultFetchStrategy)
        {
            DataSource = dataSource;
            DefaultFetchStrategy = defaultFetchStrategy;
        }

        public GetFirstQueryHandler(DataSourceFactory dataSourceFactory, IFetchStrategy<TEntity> defaultFetchStrategy)
            : this(dataSourceFactory.GetForEntity<TEntity>(), defaultFetchStrategy)
        {
        }

        public EfDataSourceBased DataSource { get; }
        public IFetchStrategy<TEntity> DefaultFetchStrategy { get; }

        protected IQueryable<TEntity> PrepareQuery(GetFirstQuery<TEntity> query)
        {
            return DataSource.Query<TEntity>()
                .MaybeWhere(query.Specification)
                .MaybeSort(query.Sorting)
                .ApplyFetchStrategy(query.FetchStrategy ?? DefaultFetchStrategy, DataSource._dbContext);
        }

        public TEntity Request(GetFirstQuery<TEntity> query)
        {
            return PrepareQuery(query).FirstOrDefault();
        }

        public async Task<TEntity> RequestAsync(GetFirstQuery<TEntity> query)
        {
            return await PrepareQuery(query).FirstOrDefaultAsync();
        }
    }
}

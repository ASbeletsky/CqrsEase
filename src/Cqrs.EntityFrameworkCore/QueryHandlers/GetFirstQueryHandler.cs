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
        public GetFirstQueryHandler(EfDataSourceBased dataSource)
        {
            DataSource = dataSource;
        }

        public GetFirstQueryHandler(DataSourceFactory dataSourceFactory)
            : this(dataSourceFactory.GetForEntity<TEntity>())
        {
        }

        public EfDataSourceBased DataSource { get; }

        protected virtual IQueryable<TEntity> GetSourceCollection(GetFirstQuery<TEntity> query)
        {
            return DataSource.Query<TEntity>();
        }

        protected IQueryable<TEntity> PrepareQuery(GetFirstQuery<TEntity> query)
        {
            return GetSourceCollection(query)
                .MaybeWhere(query.Specification)
                .MaybeSort(query.Sorting)
                .ApplyFetchStrategy(query.FetchStrategy, DataSource._dbContext);
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

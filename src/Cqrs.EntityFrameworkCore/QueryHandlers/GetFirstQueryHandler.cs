namespace Cqrs.EntityFrameworkCore.QueryHandlers
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Core.Abstractions;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
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

        public EfDataSourceBased DataSource { get; }

        protected IQueryable<TEntity> PrepareQuery(GetFirstQuery<TEntity> query)
        {
            return DataSource.Query<TEntity>()
                .MaybeWhere(query.Specification)
                .MaybeSort(query.Sorting)
                .ApplyFetchStrategy(query.FetchStrategy);
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

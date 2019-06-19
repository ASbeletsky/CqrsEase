namespace CqrsEase.EntityFrameworkCore.QueryHandlers
{
    #region Using
    using CqrsEase.Common.Queries;
    using CqrsEase.Core.Abstractions;
    using CqrsEase.EntityFrameworkCore.DataSource;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    #endregion

    public class CountQueryHandler<T>
        : IQueryHandler<CountQuery<T>, long>
        , IQueryHandlerAsync<CountQuery<T>, long>
        where T : class
    {
        public CountQueryHandler(EfDataSourceBased dataSource)
        {
            DataSource = dataSource;
        }

        public CountQueryHandler(DataSourceFactory dataSourceFactory)
            : this(dataSourceFactory.GetForEntity<T>())
        {
        }

        public EfDataSourceBased DataSource { get; }

        public long Request(Common.Queries.CountQuery<T> query)
        {
            return DataSource.Query<T>().MaybeWhere(query.Specification).Count();
        }

        public async Task<long> RequestAsync(Common.Queries.CountQuery<T> query)
        {
            return await DataSource.Query<T>().MaybeWhere(query.Specification).CountAsync();
        }
    }
}

namespace Cqrs.EntityFrameworkCore.QueryHandlers
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Core.Abstractions;
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

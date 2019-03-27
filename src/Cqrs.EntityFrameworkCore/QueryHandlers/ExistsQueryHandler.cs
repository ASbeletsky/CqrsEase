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

    public class ExistsQueryHandler<T>
        : IQueryHandler<ExistsQuery<T>, bool>
        , IQueryHandlerAsync<ExistsQuery<T>, bool>
        where T : class
    {
        public ExistsQueryHandler(EfDataSourceBased dataSource)
        {
            DataSource = dataSource;
        }

        public ExistsQueryHandler(DataSourceFactory dataSourceFactory)
            : this(dataSourceFactory.GetForEntity<T>())
        {
        }

        public EfDataSourceBased DataSource { get; }

        public bool Request(ExistsQuery<T> query)
        {
            return DataSource.Query<T>().MaybeWhere(query.Specification).Any();
        }

        public Task<bool> RequestAsync(ExistsQuery<T> query)
        {
            return DataSource.Query<T>().MaybeWhere(query.Specification).AnyAsync();
        }
    }
}

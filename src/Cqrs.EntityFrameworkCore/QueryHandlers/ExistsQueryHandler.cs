namespace Cqrs.EntityFrameworkCore.QueryHandlers
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Core.Abstractions;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    #endregion

    internal class ExistsQueryHandler<T>
        : IQueryHandler<ExistsQuery<T>, bool>
        , IQueryHandlerAsync<ExistsQuery<T>, bool>
        where T : class
    {
        public ExistsQueryHandler(EfDataSourceBased dataSource)
        {
            DataSource = dataSource;
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

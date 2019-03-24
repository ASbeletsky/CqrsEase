namespace Cqrs.EntityFrameworkCore.QueryHandlers
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Core.Abstractions;
    using Microsoft.EntityFrameworkCore;
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
        public GetManyQueryHandler(EfDataSourceBased dataSource)
        {
            DataSource = dataSource;
        }

        public EfDataSourceBased DataSource { get; }

        protected virtual IQueryable<T> PrepareFilter(GetManyQuery<T> query)
        {
            return DataSource.Query<T>().MaybeWhere(query.Specification);
        }

        protected IQueryable<T> PrepareDataQuery(GetManyQuery<T> query)
        {
            return PrepareFilter(query).MaybeSort(query.Sorting).MaybeTake(query.Pagination).ApplyFetchStrategy(query.FetchStrategy);
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
            var count = PrepareFilter(query).Count();
            IEnumerable<T> data = count > 0 ? Request(query) : Enumerable.Empty<T>();
            return new LimitedEnumerable<T>(data, count);
        }

        async Task<ILimitedEnumerable<T>> IQueryHandlerAsync<GetManyQuery<T>, ILimitedEnumerable<T>>.RequestAsync(GetManyQuery<T> query)
        {
            var count = await PrepareFilter(query).CountAsync();
            IEnumerable<T> data = count > 0 ? await RequestAsync(query) : Enumerable.Empty<T>();
            return new LimitedEnumerable<T>(data, count);
        }
    }
}

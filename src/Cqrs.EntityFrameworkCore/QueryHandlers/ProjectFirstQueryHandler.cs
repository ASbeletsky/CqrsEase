namespace Cqrs.EntityFrameworkCore.QueryHandlers
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Core.Abstractions;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    #endregion

    internal class ProjectFirstQueryHandler<TSource, TDest>
        : IQueryHandler<ProjectFirstQuery<TSource, TDest>, TDest>
        , IQueryHandlerAsync<ProjectFirstQuery<TSource, TDest>, TDest>
        where TSource : class
    {
        public ProjectFirstQueryHandler(EfDataSourceBased dataSource, IProjector projector)
        {
            DataSource = dataSource;
            Projector = projector;
        }

        public EfDataSourceBased DataSource { get; }
        public IProjector Projector { get; }

        protected IQueryable<TDest> PrepareQuery(ProjectFirstQuery<TSource, TDest> query)
        {
            return Projector.ProjectTo<TDest>(DataSource.Query<TSource>()).MaybeWhere(query.Specification).MaybeSort(query.Sorting).ApplyFetchStrategy(query.FetchStrategy);
        }

        public TDest Request(ProjectFirstQuery<TSource, TDest> query)
        {
            return PrepareQuery(query).FirstOrDefault();
        }

        public Task<TDest> RequestAsync(ProjectFirstQuery<TSource, TDest> query)
        {
            return PrepareQuery(query).FirstOrDefaultAsync();
        }
    }
}

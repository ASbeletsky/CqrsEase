namespace Cqrs.Core.Abstractions
{
    /// <summary>
    /// Defines how to execute a query.
    /// </summary>
    /// <typeparam name="TQuery">Query type</typeparam>
    /// <typeparam name="TResult">Query result type</typeparam>
    public interface IQueryHandler<in TQuery, out TResult>
        where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Requests specified query from the system.
        /// </summary>
        /// <param name="query">The query to request</param>
        /// <returns>Result of requested query</returns>
        TResult Request(TQuery query);
    }
}

namespace CqrsEase.Core.Abstractions
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines how to execute a query asynchronously.
    /// </summary>
    /// <typeparam name="TQuery">Query type</typeparam>
    /// <typeparam name="TResult">Query result type</typeparam>
    public interface IQueryHandlerAsync<in TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Requests specified query from the system asynchronously.
        /// </summary>
        /// <param name="query">The query to request</param>
        /// <returns>Result of requested query</returns>
        Task<TResult> RequestAsync(TQuery query);
    }
}

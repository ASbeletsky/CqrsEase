namespace Cqrs.Core.Abstractions
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides a mechanism for executing queries.
    /// </summary>
    public interface IQueryProcessor
    {
        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>Result of executed query</returns>
        TResult Execute<TResult>(IQuery<TResult> query);

        /// <summary>
        /// Executes a query asynchronously.
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>Result of executed query</returns>
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query);
    }
}

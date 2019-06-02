namespace Cqrs.Core
{
    #region Using
    using Cqrs.Core.Abstractions;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    #endregion

    /// <summary>
    /// Provides a mechanism for executing queries.
    /// </summary>
    public class QueryProcessor : IQueryProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryProcessor"/> class.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> instance with registred query handlers of appropriate queries. /></param>
        public QueryProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>Result of executed query</returns>
        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetService(handlerType) ?? throw new ArgumentNullException($"Query handler is not registred for query of type { query.GetType().Name }");
            return (TResult)handlerType.GetTypeInfo().GetDeclaredMethod("Request").Invoke(handler, new object[] { query });
        }

        /// <summary>
        /// Executes a query asynchronously.
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>Result of executed query</returns>
        public async Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandlerAsync<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetService(handlerType) ?? throw new ArgumentNullException($"Async Query handler is not registred for query of type { query.GetType().Name }");
            return await (Task<TResult>)handlerType.GetTypeInfo().GetDeclaredMethod("RequestAsync").Invoke(handler, new object[] { query });
        }
    }
}

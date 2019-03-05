namespace Cqrs.Core
{
    #region Using
    using Cqrs.Core.Abstractions;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    #endregion

    public class QueryProcessor : IQueryProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        public QueryProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetService(handlerType) ?? throw new ArgumentNullException($"Query handler is not registred for query of type { query.GetType().Name }");
            return (TResult)handlerType.GetTypeInfo().GetDeclaredMethod("Query").Invoke(handler, new object[] { query });
        }

        public async Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandlerAsync<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetService(handlerType) ?? throw new ArgumentNullException($"Async Query handler is not registred for query of type { query.GetType().Name }");
            return await (Task<TResult>)handlerType.GetTypeInfo().GetDeclaredMethod("QueryAsync").Invoke(handler, new object[] { query });
        }
    }
}

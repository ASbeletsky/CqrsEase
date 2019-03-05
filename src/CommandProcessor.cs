namespace Cqrs.Core
{
    #region Using
    using Cqrs.Core.Abstractions;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    #endregion

    public class CommandProcessor : ICommandProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        public CommandProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Execute(ICommand command)
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var handler = _serviceProvider.GetService(handlerType) ?? throw new ArgumentNullException($"Command handler is not registred for command of type { command.GetType().Name }");
            handlerType.GetTypeInfo().GetDeclaredMethod("Execute").Invoke(handler, new object[] { command });
        }

        public TResult Execute<TResult>(ICommand<TResult> command)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetService(handlerType) ?? throw new ArgumentNullException($"Command handler is not registred for command of type { command.GetType().Name }");
            return (TResult)handlerType.GetTypeInfo().GetDeclaredMethod("Execute").Invoke(handler, new object[] { command });
        }

        public async Task ExecuteAsync(ICommand command)
        {
            var handlerType = typeof(ICommandHandlerAsync<>).MakeGenericType(command.GetType());
            var handler = _serviceProvider.GetService(handlerType) ?? throw new ArgumentNullException($"Async Command handler is not registred for command of type { command.GetType().Name }");
            await(Task) handlerType.GetTypeInfo().GetDeclaredMethod("ExecuteAsync").Invoke(handler, new object[] { command });
        }

        public async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            var handlerType = typeof(ICommandHandlerAsync<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetService(handlerType) ?? throw new ArgumentNullException($"Async Command handler is not registred for command of type { command.GetType().Name }");
            return await(Task<TResult>) handlerType.GetTypeInfo().GetDeclaredMethod("ExecuteAsync").Invoke(handler, new object[] { command });
        }
    }
}

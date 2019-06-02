namespace Cqrs.Core
{
    #region Using
    using Cqrs.Core.Abstractions;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    #endregion

    /// <summary>
    /// Provides a mechanism for executing commands.
    /// </summary>
    public class CommandProcessor : ICommandProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessor"/> class.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> instance with registred command handlers of appropriate commands. /></param>
        public CommandProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="command">The command to execute</param>
        public void Execute(ICommand command)
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var handler = _serviceProvider.GetService(handlerType) ?? throw new ArgumentNullException($"Command handler is not registred for command of type { command.GetType().Name }");
            handlerType.GetTypeInfo().GetDeclaredMethod("Apply").Invoke(handler, new object[] { command });
        }

        /// <summary>
        /// Executes a command with particular result.
        /// </summary>
        /// <typeparam name="TResult">Command result type</typeparam>
        /// <param name="command">The command to execute</param>
        /// <returns>Result of the executed command</returns>
        public TResult Execute<TResult>(ICommand<TResult> command)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetService(handlerType) ?? throw new ArgumentNullException($"Command handler is not registred for command of type { command.GetType().Name }");
            return (TResult)handlerType.GetTypeInfo().GetDeclaredMethod("Apply").Invoke(handler, new object[] { command });
        }

        /// <summary>
        /// Executes a command asynchronously.
        /// </summary>
        /// <param name="command">The command to execute</param>
        public async Task ExecuteAsync(ICommand command)
        {
            var handlerType = typeof(ICommandHandlerAsync<>).MakeGenericType(command.GetType());
            var handler = _serviceProvider.GetService(handlerType) ?? throw new ArgumentNullException($"Async Command handler is not registred for command of type { command.GetType().Name }");
            await(Task) handlerType.GetTypeInfo().GetDeclaredMethod("ApplyAsync").Invoke(handler, new object[] { command });
        }

        /// <summary>
        /// Executes a command with particular result asynchronously.
        /// </summary>
        /// <typeparam name="TResult">Command result type</typeparam>
        /// <param name="command">The command to execute</param>
        /// <returns>Result of the executed command</returns>
        public async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            var handlerType = typeof(ICommandHandlerAsync<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetService(handlerType) ?? throw new ArgumentNullException($"Async Command handler is not registred for command of type { command.GetType().Name }");
            return await(Task<TResult>) handlerType.GetTypeInfo().GetDeclaredMethod("ApplyAsync").Invoke(handler, new object[] { command });
        }
    }
}

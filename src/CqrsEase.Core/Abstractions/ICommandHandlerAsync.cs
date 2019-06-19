namespace CqrsEase.Core.Abstractions
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines how to execute a command asynchronously.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface ICommandHandlerAsync<in TCommand>
        where TCommand : ICommand
    {
        /// <summary>
        /// Applies specified command to the system asynchronously.
        /// </summary>
        /// <param name="command">The command to apply</param>
        Task ApplyAsync(TCommand command);
    }

    /// <summary>
    /// Defines how to execute a command with particular result asynchronously.
    /// </summary>
    /// <typeparam name="TCommand">Command type</typeparam>
    /// <typeparam name="TResult">Command result type</typeparam>
    public interface ICommandHandlerAsync<in TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        /// <summary>
        /// Applies specified command to the system asynchronously.
        /// </summary>
        /// <param name="command">The command to apply</param>
        /// <returns>Result of applied command</returns>
        Task<TResult> ApplyAsync(TCommand command);
    }
}

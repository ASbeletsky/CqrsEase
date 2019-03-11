namespace Cqrs.Core.Abstractions
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides a mechanism for executing commands.
    /// </summary>
    public interface ICommandProcessor
    {
        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="command">The command to execute</param>
        void Execute(ICommand command);

        /// <summary>
        /// Executes a command with particular result.
        /// </summary>
        /// <typeparam name="TResult">Command result type</typeparam>
        /// <param name="command">The command to execute</param>
        /// <returns>Result of the executed command</returns>
        TResult Execute<TResult>(ICommand<TResult> command);

        /// <summary>
        /// Executes a command asynchronously.
        /// </summary>
        /// <param name="command">The command to execute</param>
        Task ExecuteAsync(ICommand command);

        /// <summary>
        /// Executes a command with particular result asynchronously.
        /// </summary>
        /// <typeparam name="TResult">Command result type</typeparam>
        /// <param name="command">The command to execute</param>
        /// <returns>Result of the executed command</returns>
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command);
    }
}

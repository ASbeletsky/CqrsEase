namespace Cqrs.Core.Abstractions
{
    /// <summary>
    /// Defines how to execute a command.
    /// </summary>
    /// <typeparam name="TCommand">Command type</typeparam>
    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        /// <summary>
        /// Applies specified command to the system.
        /// </summary>
        /// <param name="command">The command to apply</param>
        void Apply(TCommand command);
    }

    /// <summary>
    /// Defines how to apply a command with particular result.
    /// </summary>
    /// <typeparam name="TCommand">Command type</typeparam>
    /// <typeparam name="TResult">Command result type</typeparam>
    public interface ICommandHandler<in TCommand, out TResult>
        where TCommand : ICommand<TResult>
    {
        /// <summary>
        /// Applies specified command to the system.
        /// </summary>
        /// <param name="command">The command to apply</param>
        /// <returns>Result of applied command</returns>
        TResult Apply(TCommand command);
    }
}

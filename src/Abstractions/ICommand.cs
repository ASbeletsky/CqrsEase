namespace Cqrs.Core.Abstractions
{
    /// <summary>
    /// Represents a write operation that changes state of the system.
    /// </summary>
    public interface ICommand
    {
    }

    /// <summary>
    /// Represents a write operation that changes state of the system with partilar result.
    /// </summary>
    /// <typeparam name="TResult">Command result type</typeparam>
    public interface ICommand<out TResult>
    {
    }
}

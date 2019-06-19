namespace CqrsEase.Core.Abstractions
{
    /// <summary>
    /// Represents a read operation that retrieves data from the system without changing its state.
    /// </summary>
    /// <typeparam name="TResult">Query result type</typeparam>
    public interface IQuery<out TResult>
    {
    }
}

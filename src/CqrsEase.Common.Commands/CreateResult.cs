namespace CqrsEase.Common.Commands
{
    /// <summary>
    /// Represents a result of operation for creating a new data in the system.
    /// </summary>
    /// <typeparam name="T">The type of data to create in the system.</typeparam>
    public interface ICreateResult<T>
    {
        /// <summary>
        /// The data that was created in the system.
        /// </summary>
        T CreatedValue { get; }
    }

    /// <summary>
    /// Represents a result of operation for creating a new data in the system.
    /// </summary>
    /// <typeparam name="T">The type of data to create in the system.</typeparam>
    public class CreateResult<T> : ICreateResult<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateResult{T}{T}"/> class.
        /// </summary>
        /// <param name="createdValue">The data that was created in the system.</param>
        public CreateResult(T createdValue)
        {
            CreatedValue = createdValue;
        }

        /// <summary>
        /// The data that was created in the system.
        /// </summary>
        public T CreatedValue { get; }
    }
}

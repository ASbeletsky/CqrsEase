namespace Cqrs.Common.Commands
{
    using Cqrs.Core.Abstractions;

    /// <summary>
    /// Represents a command for creating a new data in the system.
    /// </summary>
    /// <typeparam name="T">The type of data to create in the system.</typeparam>
    public class CreateCommand<T>
        : ICommand
        , ICommand<ICreateResult<T>>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommand{T}"/> class.
        /// </summary>
        /// <param name="value">A data to create in the system.</param>
        public CreateCommand(T value)
        {
            Value = value;
        }

        /// <summary>
        /// The data to create in the system.
        /// </summary>
        public T Value { get; }
    }  
}

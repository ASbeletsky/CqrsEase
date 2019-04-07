namespace Cqrs.Common.Commands
{
    #region Using
    using Cqrs.Core.Abstractions;
    using NSpecifications;
    #endregion

    /// <summary>
    /// Represents a command for removing an existing data in the system.
    /// </summary>
    /// <typeparam name="T">The type of data that should be removed.</typeparam>
    public class DeleteCommand<T>
        : ICommand
        , ICommand<IDeleteResult>
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="DeleteCommand{T}"/> class.
        /// </summary>
        /// <param name="applyTo">The selection criteria for data to remove.</param>
        /// <param name="deleteFirstMatchOnly">If set to true, removes only first object that meet the <paramref name="applyTo"/> criteria. If set to false, removes multiple objects.</param>
        public DeleteCommand(ISpecification<T> applyTo, bool deleteFirstMatchOnly = true)
        {
            ApplyTo = applyTo;
            DeleteFirstMatchOnly = deleteFirstMatchOnly;
        }

        /// <summary>
        /// The selection criteria for data to remove.
        /// </summary>
        public ISpecification<T> ApplyTo { get; }

        /// <summary>
        /// Indicates whether only the first object that meet the selection criteria should be removed.
        /// </summary>
        public bool DeleteFirstMatchOnly { get; }
    }
}

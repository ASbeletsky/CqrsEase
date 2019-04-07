namespace Cqrs.Common.Commands
{
    #region Using
    using Cqrs.Core.Abstractions;
    using NSpecifications;
    #endregion

    /// <summary>
    /// Represents a command for updating an existing data in the system using specified value.
    /// </summary>
    /// <typeparam name="T">The type of data that should be updated.</typeparam>
    public class UpdateCommand<T> : UpdateCommand<T, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommand{T}"/> class.
        /// </summary>
        /// <param name="applyTo">The selection criteria for data to update.</param>
        /// <param name="value">The modifications to apply.</param>
        /// <param name="updateFirstMatchOnly">If set to true, updates only first object that meet the <paramref name="applyTo"/> criteria. If set to false, updates multiple objects.</param>
        public UpdateCommand(ISpecification<T> applyTo, T value, bool updateFirstMatchOnly = true)
            : base(applyTo, value, updateFirstMatchOnly)
        {
        }
    }

    /// <summary>
    /// Represents a command for updating an existing data in the system using specified value.
    /// The value can be either anonymous type, type mapped by automapper from <typeparamref name="TData"/> type or any other assignable type from <typeparamref name="TData"/> type. 
    /// </summary>
    /// <typeparam name="TData">The type of data to update in the system.</typeparam>
    /// <typeparam name="TValue">The type of value used to update existing data.</typeparam>
    public class UpdateCommand<TData, TValue>
        : ICommand
        , ICommand<IUpdateResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommand{TData, TValue}"/> class.
        /// </summary>
        /// <param name="applyTo">The selection criteria for data to update.</param>
        /// <param name="value">The modifications to apply.</param>
        /// <param name="updateFirstMatchOnly">If set to true, updates only first object that meet the <paramref name="applyTo"/> criteria. If set to false, updates multiple objects.</param>
        public UpdateCommand(ISpecification<TData> applyTo, TValue value, bool updateFirstMatchOnly = true)
        {
            ApplyTo = applyTo;
            Value = value;
            UpdateFirstMatchOnly = updateFirstMatchOnly;
        }

        /// <summary>
        /// The selection criteria for data to update.
        /// </summary>
        public ISpecification<TData> ApplyTo { get; }

        /// <summary>
        /// The modifications to apply.
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Indicates whether only the first object that meet the selection criteria should be updated.
        /// </summary>
        public bool UpdateFirstMatchOnly { get; }  
    }
}

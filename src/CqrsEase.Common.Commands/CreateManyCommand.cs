namespace CqrsEase.Common.Commands
{
    #region Using
    using CqrsEase.Core.Abstractions;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// Represents a command for creating multiple data objects in the system.
    /// </summary>
    /// <typeparam name="T">The type of data to create in the system.</typeparam>
    public class CreateManyCommand<T> : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateManyCommand{T}"/> class.
        /// </summary>
        /// <param name="valuesParams">A range of objects params to create in the system.</param>
        public CreateManyCommand(params T[] valuesParams) : this(values: valuesParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateManyCommand{T}"/> class.
        /// </summary>
        /// <param name="values">A range of objects to create in the system.</param>
        public CreateManyCommand(IEnumerable<T> values)
        {
            Values = values;
        }

        /// <summary>
        /// The range of objects to create.
        /// </summary>
        public IEnumerable<T> Values { get; }
    }
}

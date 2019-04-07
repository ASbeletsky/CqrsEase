namespace Cqrs.Common.Commands
{
    /// <summary>
    /// Represents a result of operation for updating an existing data in the system.
    /// </summary>
    public interface IUpdateResult
    {
        /// <summary>
        /// The number of objects that was updated.
        /// </summary>
        int UpdatedCount { get; }
    }

    /// <summary>
    /// Represents a result of operation for updating an existing data in the system.
    /// </summary>
    public class UpdateResult : IUpdateResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateResult"/> class.
        /// </summary>
        /// <param name="updatedCount">The number of objects that was updated.</param>
        public UpdateResult(int updatedCount)
        {
            UpdatedCount = updatedCount;
        }

        /// <summary>
        /// The number of objects that was updated.
        /// </summary>
        public int UpdatedCount { get; }
    }
}

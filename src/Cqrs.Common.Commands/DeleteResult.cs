namespace Cqrs.Common.Commands
{
    /// <summary>
    /// Represents a result of operation for removing an existing data in the system.
    /// </summary>
    public interface IDeleteResult
    {
        /// <summary>
        /// The number of objects that was deleted.
        /// </summary>
        int DeletedCount { get; }
    }

    /// <summary>
    /// Represents a result of operation for removing an existing data in the system.
    /// </summary>
    public class DeleteResult : IDeleteResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteResult"/> class.
        /// </summary>
        /// <param name="updatedCount">The number of objects that was deleted.</param>
        public DeleteResult(int updatedCount)
        {
            DeletedCount = updatedCount;
        }

        /// <summary>
        /// The number of objects that was deleted.
        /// </summary>
        public int DeletedCount { get; }
    }
}

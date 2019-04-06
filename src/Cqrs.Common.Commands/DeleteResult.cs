namespace Cqrs.Common.Commands
{
    public interface IDeleteResult
    {
        int UpdatedCount { get; }
    }

    public class DeleteResult : IDeleteResult
    {
        public DeleteResult(int updatedCount)
        {
            UpdatedCount = updatedCount;
        }

        public int UpdatedCount { get; }
    }
}

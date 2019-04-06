namespace Cqrs.Common.Commands
{
    public interface IUpdateResult
    {
        int UpdatedCount { get; }
    }

    public class UpdateResult : IUpdateResult
    {
        public UpdateResult(int updatedCount)
        {
            UpdatedCount = updatedCount;
        }

        public int UpdatedCount { get; }
    }
}

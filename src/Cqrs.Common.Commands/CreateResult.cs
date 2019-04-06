namespace Cqrs.Common.Commands
{
    public interface ICreateResult<T>
    {
        T CreatedValue { get; }
    }

    public class CreateResult<T> : ICreateResult<T> where T : class
    {
        public CreateResult(T createdValue)
        {
            CreatedValue = createdValue;
        }

        public T CreatedValue { get; }
    }
}

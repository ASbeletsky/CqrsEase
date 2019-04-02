namespace Cqrs.Common.Commands
{
    using Cqrs.Core.Abstractions;

    public class CreateCommand<T>
        : ICommand
        , ICommand<ICreateResult<T>>
        where T : class
    {
        public CreateCommand(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }  
}

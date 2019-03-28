namespace Cqrs.Common.Commands
{
    using Cqrs.Core.Abstractions;

    public class CreateCommand<T> : ICommand
    {
        public CreateCommand(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }

    public class CreateCommand<T, TResult> : CreateCommand<T>, ICommand<TResult>
    {
        public CreateCommand(T value) : base(value)
        {
        }
    }
}

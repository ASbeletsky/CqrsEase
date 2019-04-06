namespace Cqrs.Common.Commands
{
    using Cqrs.Core.Abstractions;
    using NSpecifications;

    public class UpdateCommand<T>
        : ICommand
        , ICommand<IUpdateResult>
    {
        public UpdateCommand(ISpecification<T> applyTo, T value, bool updateFirstMatchOnly = true)
        {
            ApplyTo = applyTo;
            Value = value;
            UpdateFirstMatchOnly = updateFirstMatchOnly;
        }

        public ISpecification<T> ApplyTo { get; }
        public T Value { get; }
        public bool UpdateFirstMatchOnly { get; }
    }
}

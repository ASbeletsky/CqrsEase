namespace Cqrs.Common.Commands
{
    using Cqrs.Core.Abstractions;
    using NSpecifications;

    public class UpdateCommand<T> : ICommand
    {
        public UpdateCommand(ISpecification<T> applyTo, T value, bool updateFirstMatchOnly = true)
        {
            ApplyTo = applyTo;
            Value = value;
            UpdateFirstMatchOnly = updateFirstMatchOnly;
        }

        public ISpecification<T> ApplyTo { get; }
        public T Value { get; }
        protected bool UpdateFirstMatchOnly { get; }
    }

    public class UpdateCommand<T, TResult> : UpdateCommand<T>, ICommand<TResult>
    {
        public UpdateCommand(ISpecification<T> applyTo, T value) : base(applyTo, value)
        {
        }
    }
}

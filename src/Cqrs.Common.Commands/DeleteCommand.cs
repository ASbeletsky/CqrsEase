namespace Cqrs.Common.Commands
{
    using Cqrs.Core.Abstractions;
    using NSpecifications;

    public class DeleteCommand<T> : ICommand
    {
        public DeleteCommand(ISpecification<T> applyTo)
        {
            ApplyTo = applyTo;
        }

        public ISpecification<T> ApplyTo { get; }
    }

    public class DeleteCommand<T, TResult> : DeleteCommand<T>, ICommand<TResult>
    {
        public DeleteCommand(ISpecification<T> applyTo) : base(applyTo)
        {
        }
    }
}

namespace Cqrs.Common.Commands
{
    using Cqrs.Core.Abstractions;
    using NSpecifications;

    public class DeleteCommand<T>
        : ICommand
        , ICommand<IDeleteResult>
    {
        public DeleteCommand(ISpecification<T> applyTo, bool deleteFirstMatchOnly = true)
        {
            ApplyTo = applyTo;
            DeleteFirstMatchOnly = deleteFirstMatchOnly;
        }

        public ISpecification<T> ApplyTo { get; }
        public bool DeleteFirstMatchOnly { get; }
    }
}

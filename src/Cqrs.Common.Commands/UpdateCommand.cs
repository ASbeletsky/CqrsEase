namespace Cqrs.Common.Commands
{
    using Cqrs.Core.Abstractions;
    using NSpecifications;

    public class UpdateCommand<T> : UpdateCommand<T, T>
    {
        public UpdateCommand(ISpecification<T> applyTo, T value, bool updateFirstMatchOnly = true)
            : base(applyTo, value, updateFirstMatchOnly)
        {
        }
    }

    public class UpdateCommand<TDest, TSource>
        : ICommand
        , ICommand<IUpdateResult>
    {
        public UpdateCommand(ISpecification<TDest> applyTo, TSource value, bool updateFirstMatchOnly = true)
        {
            ApplyTo = applyTo;
            Value = value;
            UpdateFirstMatchOnly = updateFirstMatchOnly;
        }

        public ISpecification<TDest> ApplyTo { get; }
        public TSource Value { get; }
        public bool UpdateFirstMatchOnly { get; }  
    }
}

using NSpecifications;

namespace Cqrs.Common.Commands
{
    public class UpdateManyCommand<T> : UpdateCommand<T>
    {
        public UpdateManyCommand(ISpecification<T> applyTo, T value) : base(applyTo, value, updateFirstMatchOnly : false)
        {
        }
    }
}

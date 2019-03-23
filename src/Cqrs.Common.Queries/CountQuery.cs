using Cqrs.Core.Abstractions;
using NSpecifications;

namespace Cqrs.Common.Queries
{
    public class CountQuery<T> : IQuery<long>
    {
        public CountQuery(ISpecification<T> specification)
        {
            Specification = specification;
        }

        public ISpecification<T> Specification { get; }
    }
}

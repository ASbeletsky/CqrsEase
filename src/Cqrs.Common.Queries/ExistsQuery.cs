using Cqrs.Core.Abstractions;
using NSpecifications;

namespace Cqrs.Common.Queries
{
    public class ExistsQuery<T> : IQuery<bool>
    {
        public ExistsQuery(ISpecification<T> specification)
        {
            Specification = specification;
        }

        public ISpecification<T> Specification { get; }
    }
}

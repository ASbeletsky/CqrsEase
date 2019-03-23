using Cqrs.Core.Abstractions;
using NSpecifications;

namespace Cqrs.Common.Queries
{
    public class ExistsQuery<T, TSpecification> : IQuery<bool>
            where TSpecification : ISpecification<T>
    {
        public ExistsQuery(TSpecification specification)
        {
            Specification = specification;
        }

        public TSpecification Specification { get; }
    }
}

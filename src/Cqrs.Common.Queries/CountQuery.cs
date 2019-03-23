using Cqrs.Core.Abstractions;
using NSpecifications;

namespace Cqrs.Common.Queries
{
    public class CountQuery<T, TSpecification> : IQuery<long>
        where TSpecification : ISpecification<T>
    {
        public CountQuery(TSpecification specification)
        {
            Specification = specification;
        }

        public TSpecification Specification { get; }
    }
}

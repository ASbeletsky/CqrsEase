using Cqrs.Common.Queries.Sorting;
using Cqrs.Core.Abstractions;
using Cqrs.Model.Abstractions;
using NSpecifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cqrs.Common.Queries
{
    public class FindFirstQuery<T, TSpecification> : IQuery<T>
        where TSpecification : ISpecification<T>
    {
        public FindFirstQuery(TSpecification specification)
            : this(specification, null)
        {

        }

        public FindFirstQuery(TSpecification specification, IOrderedFetchStrategy<T> fetchStrategy)
        {
            Specification = specification;
            FetchStrategy = fetchStrategy;
        }

        public TSpecification Specification { get; }

        public IOrderedFetchStrategy<T> FetchStrategy { get; set; }
    }
}

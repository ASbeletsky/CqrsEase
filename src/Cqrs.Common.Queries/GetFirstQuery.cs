namespace Cqrs.Common.Queries
{
    #region Using
    using Cqrs.Common.Queries.FetchStrategies;
    using Cqrs.Common.Queries.Sorting;
    using Cqrs.Core.Abstractions;
    using NSpecifications;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// Represents a query for reading first matched data obejct from the system.
    /// </summary>
    /// <typeparam name="T">The type of data to read.</typeparam>
    public class GetFirstQuery<T> : IQuery<T>
    {
        #region Reading use cases

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFirstQuery{T}"/> class.
        /// Returns first <typeparamref name="T"/> object according to the specified <paramref name="sortingParams"/> parameter.
        /// </summary>
        /// <param name="sortingParams">The ordering used to sort <typeparamref name="T"/> objects.</param> 
        public GetFirstQuery(params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: null, sortingParams: sortingParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFirstQuery{T}"/> class.
        /// Returns first <typeparamref name="T"/> object according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// Includes only those <typeparamref name="T"/> members that was specified in <paramref name="fetchStrategy"/> parameter.
        /// </summary>
        /// <param name="fetchStrategy">The members of <typeparamref name="T"/> to populate.</param>
        /// <param name="sortingParams">The ordering used to sort <typeparamref name="T"/> objects.</param>        
        public GetFirstQuery(IFetchStrategy<T> fetchStrategy, params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: fetchStrategy, specification: null, orderBy: sortingParams)

        {
        }

        #endregion

        #region Filter use cases

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFirstQuery{T}"/> class.
        /// Returns first <typeparamref name="T"/> object that satisfied the <paramref name="specification"/> creteria.
        /// </summary>
        /// <param name="specification">The creteria used to filter <typeparamref name="T"/> objects.</param>
        public GetFirstQuery(ISpecification<T> specification)
            : this(specification: specification, sortingParams: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFirstQuery{T}"/> class.
        /// Returns first <typeparamref name="T"/> object that satisfied the <paramref name="specification"/> creteria according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// </summary>
        /// <param name="specification">The creteria used to filter <typeparamref name="T"/> objects.</param>
        /// <param name="sortingParams">The ordering used to sort <typeparamref name="T"/> objects.</param>
        public GetFirstQuery(ISpecification<T> specification, params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: null, specification: specification, sortingParams: sortingParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFirstQuery{T}"/> class.
        /// Returns first <typeparamref name="T"/> object that satisfied the <paramref name="specification"/> creteria according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// Includes only those <typeparamref name="T"/> members that was specified in <paramref name="fetchStrategy"/> parameter.
        /// </summary>
        /// <param name="fetchStrategy">The members of <typeparamref name="T"/> to populate.</param>
        /// <param name="specification">The creteria used to filter <typeparamref name="T"/> objects.</param>
        /// <param name="sortingParams">The ordering used to sort <typeparamref name="T"/> objects.</param>
        public GetFirstQuery(IFetchStrategy<T> fetchStrategy, ISpecification<T> specification, params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: fetchStrategy, specification: specification, orderBy: sortingParams)
        {
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFirstQuery{T}"/> class.
        /// Returns first <typeparamref name="T"/> object that satisfied the <paramref name="specification"/> creteria according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// Includes only those <typeparamref name="T"/> members that was specified in <paramref name="fetchStrategy"/> parameter.
        /// </summary>
        /// <param name="fetchStrategy">The members of <typeparamref name="T"/> to populate.</param>
        /// <param name="specification">The creteria used to filter <typeparamref name="T"/> objects.</param>
        /// <param name="orderBy">The ordering used to sort <typeparamref name="T"/> objects.</param>
        public GetFirstQuery(IFetchStrategy<T> fetchStrategy, ISpecification<T> specification,  IEnumerable<OrderCreteria<T>> orderBy)
        {
            Specification = specification;
            FetchStrategy = fetchStrategy;
            Sorting = orderBy;
        }

        /// <summary>
        /// The creteria used to filter <typeparamref name="T"/> objects.
        /// </summary>
        public ISpecification<T> Specification { get; }

        /// <summary>
        /// The members of <typeparamref name="T"/> to populate.
        /// </summary>
        public IFetchStrategy<T> FetchStrategy { get; }

        /// <summary>
        /// The ordering used to sort <typeparamref name="T"/> objects.
        /// </summary>
        public IEnumerable<OrderCreteria<T>> Sorting { get; }
    }
}

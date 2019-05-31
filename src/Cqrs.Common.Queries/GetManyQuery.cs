namespace Cqrs.Common.Queries
{
    #region Using
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Common.Queries.FetchStrategies;
    using Cqrs.Common.Queries.Sorting;
    using Cqrs.Core.Abstractions;
    using NSpecifications;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// Represents a query for reading collection of matched data obejct from the system.
    /// </summary>
    /// <typeparam name="T">The type of data to read.</typeparam>
    public class GetManyQuery<T> : IQuery<IEnumerable<T>>, IQuery<ILimitedEnumerable<T>>
    {

        #region Get all use cases

        /// <summary>
        /// Initializes a new instance of the <see cref="GetManyQuery{T}"/> class.
        /// Returns collection of <typeparamref name="T"/> objects according to the specified <paramref name="sortingParams"/> parameter.
        /// </summary>
        /// <param name="sortingParams">The ordering used to sort <typeparamref name="T"/> objects.</param>
        public GetManyQuery(params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: null, sortingParams: sortingParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetManyQuery{T}"/> class.
        /// Returns collection of <typeparamref name="T"/> objects according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// Includes only those <typeparamref name="T"/> members that was specified in <paramref name="fetchStrategy"/> parameter.
        /// </summary>
        /// <param name="fetchStrategy">The members of <typeparamref name="T"/> to populate.</param>
        /// <param name="sortingParams">The creterias used to sort collection of <typeparamref name="T"/> objects.</param>
        public GetManyQuery(IFetchStrategy<T> fetchStrategy, params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: fetchStrategy, specification: null, pagination: null, sorting: sortingParams)

        {
        }

        #endregion

        #region Filter use cases

        /// <summary>
        /// Initializes a new instance of the <see cref="GetManyQuery{T}"/> class.
        /// Returns collection of <typeparamref name="T"/> objects that satisfied the <paramref name="specification"/> creteria.
        /// </summary>
        /// <param name="specification">The creteria used to filter collection of <typeparamref name="T"/> objects.</param>
        public GetManyQuery(ISpecification<T> specification)
            : this(specification: specification, sortingParams: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetManyQuery{T}"/> class.
        /// Returns collection of <typeparamref name="T"/> objects that satisfied the <paramref name="specification"/> creteria according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// </summary>
        /// <param name="specification">The creteria used to filter collection of <typeparamref name="T"/> objects.</param>
        /// <param name="sortingParams">The creterias used to sort collection of <typeparamref name="T"/> objects.</param>
        public GetManyQuery(ISpecification<T> specification, params OrderCreteria<T>[] sortingParams)
            : this(specification: specification, pagination: null, sortingParams: sortingParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetManyQuery{T}"/> class.
        /// Returns paged collection of <typeparamref name="T"/> objects that satisfied the <paramref name="specification"/> creteria according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// </summary>
        /// <param name="specification">The creteria used to filter collection of <typeparamref name="T"/> objects.</param>
        /// <param name="pagination">The creteria used to limit rertived collection of <typeparamref name="T"/> objects to a subset.</param>
        /// <param name="sortingParams">The creterias used to sort collection of <typeparamref name="T"/> objects.</param>
        public GetManyQuery(ISpecification<T> specification, IPage pagination, params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: null, specification: specification, pagination: pagination, sorting: sortingParams)

        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetManyQuery{T}"/> class.
        /// Returns collection of <typeparamref name="T"/> objects that satisfied the <paramref name="specification"/> creteria according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// Includes only those <typeparamref name="T"/> members that was specified in <paramref name="fetchStrategy"/> parameter.
        /// </summary>
        /// <param name="fetchStrategy">The members of <typeparamref name="T"/> to populate.</param>
        /// <param name="specification">The creteria used to filter collection of <typeparamref name="T"/> objects.</param>
        /// <param name="sortingParams">The creterias used to sort collection of <typeparamref name="T"/> objects.</param>
        public GetManyQuery(IFetchStrategy<T> fetchStrategy, ISpecification<T> specification, params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: fetchStrategy, specification: specification, pagination: null, sorting: sortingParams)

        {
        }

        #endregion

        #region Paging use cases

        /// <summary>
        /// Initializes a new instance of the <see cref="GetManyQuery{T}"/> class.
        /// Returns paged collection of <typeparamref name="T"/> objects.
        /// </summary>
        /// <param name="pagination"></param>
        public GetManyQuery(IPage pagination)
            : this(pagination: pagination, sortingParams: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetManyQuery{T}"/> class.
        /// Returns paged collection of <typeparamref name="T"/> objects according to the specified <paramref name="sortingParams"/> parameter.
        /// </summary>
        /// <param name="pagination">The creteria used to limit rertived collection of <typeparamref name="T"/> objects to a subset.</param>
        /// <param name="sortingParams">The creterias used to sort collection of <typeparamref name="T"/> objects.</param>
        public GetManyQuery(IPage pagination, params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: null, pagination: pagination, sortingParams: sortingParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetManyQuery{T}"/> class.
        /// Returns paged collection of <typeparamref name="T"/> objects according to the specified <paramref name="sortingParams"/> parameter.
        /// Includes only those <typeparamref name="T"/> members that was specified in <paramref name="fetchStrategy"/> parameter.
        /// </summary>
        /// <param name="fetchStrategy">The members of <typeparamref name="T"/> to populate.</param>
        /// <param name="pagination">The creteria used to limit rertived collection of <typeparamref name="T"/> objects to a subset.</param>
        /// <param name="sortingParams">The creterias used to sort collection of <typeparamref name="T"/> objects.</param>
        public GetManyQuery(IFetchStrategy<T> fetchStrategy, IPage pagination, params OrderCreteria<T>[] sortingParams)
            : this(fetchStrategy: fetchStrategy, specification: null, pagination: pagination, sorting: sortingParams)
        {
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GetManyQuery{T}"/> class.
        /// Returns paged collection of <typeparamref name="T"/> objects that satisfied the <paramref name="specification"/> creteria according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// Includes only those <typeparamref name="T"/> members that was specified in <paramref name="fetchStrategy"/> parameter.
        /// </summary>
        /// <param name="fetchStrategy">The members of <typeparamref name="T"/> to populate.</param>
        /// <param name="specification">The creteria used to filter collection of <typeparamref name="T"/> objects.</param>
        /// <param name="pagination">The creteria used to limit rertived collection of <typeparamref name="T"/> objects to a subset.</param>
        /// <param name="sorting">The creterias used to sort collection of <typeparamref name="T"/> objects.</param>
        public GetManyQuery(IFetchStrategy<T> fetchStrategy, ISpecification<T> specification, IPage pagination, IEnumerable<OrderCreteria<T>> sorting)
        {
            Specification = specification;
            FetchStrategy = fetchStrategy;
            Sorting = sorting;
            Pagination = pagination;
        }

        /// <summary>
        /// The members of <typeparamref name="T"/> to populate.
        /// </summary>
        public IFetchStrategy<T> FetchStrategy { get; }

        /// <summary>
        /// The creteria used to filter collection of <typeparamref name="T"/> objects.
        /// </summary>
        public ISpecification<T> Specification { get; }

        /// <summary>
        /// The creterias used to sort collection of <typeparamref name="T"/> objects.
        /// </summary>
        public IEnumerable<OrderCreteria<T>> Sorting { get; }

        /// <summary>
        /// The creteria used to limit rertived collection of <typeparamref name="T"/> objects to a subset.
        /// </summary>
        public IPage Pagination { get; }
    }
}

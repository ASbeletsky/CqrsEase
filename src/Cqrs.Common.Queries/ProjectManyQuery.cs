namespace Cqrs.Common.Queries
{
    #region Using
    using System.Collections.Generic;
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Common.Queries.Sorting;
    using NSpecifications;
    #endregion

    /// <summary>
    /// Represents a query for reading collection of matched <typeparamref name="TSource"/> obejcts and projecting it to collection of <typeparamref name="TDest"/> objects.
    /// </summary>
    /// <typeparam name="TSource">The type of data to read.</typeparam>
    /// <typeparam name="TDest">The type of data to project.</typeparam>
    public class ProjectManyQuery<TSource, TDest> : GetManyQuery<TDest>
    {
        #region Project all use cases

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectManyQuery{TSource, TDest}"/> class.
        /// Returns collection of <typeparamref name="TDest"/> objects according to the specified <paramref name="sortingParams"/> parameter.
        /// </summary>
        /// <param name="sortingParams">The ordering used to sort <typeparamref name="TDest"/> objects.</param>
        public ProjectManyQuery(params OrderCreteria<TDest>[] sortingParams) : base(sortingParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectManyQuery{TSource, TDest}"/> class.
        /// Returns collection of <typeparamref name="TDest"/> objects according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// Includes only those <typeparamref name="TDest"/> members that was specified in <paramref name="fetchStrategy"/> parameter.
        /// </summary>
        /// <param name="fetchStrategy">The members of <typeparamref name="T"/> to populate.</param>
        /// <param name="sortingParams">The creterias used to sort collection of <typeparamref name="T"/> objects.</param>
        public ProjectManyQuery(IFetchStrategy<TDest> fetchStrategy, params OrderCreteria<TDest>[] sortingParams) : base(fetchStrategy, sortingParams)
        {
        }

        #endregion

        #region Filter uses cases

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectManyQuery{TSource, TDest}"/> class.
        /// Returns collection of <typeparamref name="TDest"/> objects that satisfied the <paramref name="specification"/> creteria.
        /// </summary>
        /// <param name="specification">The creteria used to filter collection of <typeparamref name="TDest"/> objects.</param>
        public ProjectManyQuery(ISpecification<TDest> specification) : base(specification)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectManyQuery{TSource, TDest}"/> class.
        /// Returns collection of <typeparamref name="TDest"/> objects that satisfied the <paramref name="specification"/> creteria according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// </summary>
        /// <param name="specification">The creteria used to filter collection of <typeparamref name="TDest"/> objects.</param>
        /// <param name="sortingParams">The creterias used to sort collection of <typeparamref name="TDest"/> objects.</param>
        public ProjectManyQuery(ISpecification<TDest> specification, params OrderCreteria<TDest>[] sortingParams) : base(specification, sortingParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectManyQuery{TSource, TDest}"/> class.
        /// Returns paged collection of <typeparamref name="TDest"/> objects that satisfied the <paramref name="specification"/> creteria according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// </summary>
        /// <param name="specification">The creteria used to filter collection of <typeparamref name="TDest"/> objects.</param>
        /// <param name="pagination">The creteria used to limit rertived collection of <typeparamref name="TDest"/> objects to a subset.</param>
        /// <param name="sortingParams">The creterias used to sort collection of <typeparamref name="TDest"/> objects.</param>
        public ProjectManyQuery(ISpecification<TDest> specification, IPage pagination, params OrderCreteria<TDest>[] sortingParams) : base(specification, pagination, sortingParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectManyQuery{TSource, TDest}"/> class.
        /// Returns collection of <typeparamref name="TDest"/> objects that satisfied the <paramref name="specification"/> creteria according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// Includes only those <typeparamref name="TDest"/> members that was specified in <paramref name="fetchStrategy"/> parameter.
        /// </summary>
        /// <param name="fetchStrategy">The members of <typeparamref name="TDest"/> to populate.</param>
        /// <param name="specification">The creteria used to filter collection of <typeparamref name="TDest"/> objects.</param>
        /// <param name="sortingParams">The creterias used to sort collection of <typeparamref name="TDest"/> objects.</param>
        public ProjectManyQuery(IFetchStrategy<TDest> fetchStrategy, ISpecification<TDest> specification, params OrderCreteria<TDest>[] sortingParams) : base(fetchStrategy, specification, sortingParams)
        {
        }

        #endregion

        #region Paging use cases

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectManyQuery{TSource, TDest}"/> class.
        /// Returns paged collection of <typeparamref name="TDest"/> objects.
        /// </summary>
        /// <param name="pagination"></param>
        public ProjectManyQuery(IPage pagination) : base(pagination)
        {
        }

        public ProjectManyQuery(IPage pagination, params OrderCreteria<TDest>[] sortingParams) : base(pagination, sortingParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectManyQuery{TSource, TDest}"/> class.
        /// Returns paged collection of <typeparamref name="TDest"/> objects according to the specified <paramref name="sortingParams"/> parameter.
        /// </summary>
        /// <param name="pagination">The creteria used to limit rertived collection of <typeparamref name="TDest"/> objects to a subset.</param>
        /// <param name="sortingParams">The creterias used to sort collection of <typeparamref name="TDest"/> objects.</param>
        public ProjectManyQuery(IFetchStrategy<TDest> fetchStrategy, IPage pagination, params OrderCreteria<TDest>[] sortingParams) : base(fetchStrategy, pagination, sortingParams)
        {
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectManyQuery{TSource, TDest}"/> class.
        /// Returns paged collection of <typeparamref name="TDest"/> objects according to the specified <paramref name="sortingParams"/> parameter.
        /// Includes only those <typeparamref name="TDest"/> members that was specified in <paramref name="fetchStrategy"/> parameter.
        /// </summary>
        /// <param name="fetchStrategy">The members of <typeparamref name="TDest"/> to populate.</param>
        /// <param name="pagination">The creteria used to limit rertived collection of <typeparamref name="TDest"/> objects to a subset.</param>
        /// <param name="sortingParams">The creterias used to sort collection of <typeparamref name="TDest"/> objects.</param>
        public ProjectManyQuery(IFetchStrategy<TDest> fetchStrategy, ISpecification<TDest> specification, IPage pagination, IEnumerable<OrderCreteria<TDest>> sorting) : base(fetchStrategy, specification, pagination, sorting)
        {
        }
    }
}

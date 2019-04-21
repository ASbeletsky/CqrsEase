namespace Cqrs.Common.Queries
{
    #region Using
    using NSpecifications;
    using Cqrs.Common.Queries.Sorting;
    using System;
    using System.Linq.Expressions;
    using Cqrs.Common.Queries.FetchStateries;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// Represents a query for reading first matched <typeparamref name="TSource"/> obejct and projecting it to <typeparamref name="TDest"/> object.
    /// </summary>
    /// <typeparam name="TSource">The type of data to read.</typeparam>
    /// <typeparam name="TDest">The type of data to project.</typeparam>
    public class ProjectFirstQuery<TSource, TDest> : GetFirstQuery<TDest>
    {
        #region Get all use cases

        /// <summary>
        /// Initializes a new instance of the <<see cref="ProjectFirstQuery{TSource, TDest}"/> class.
        /// Returns first <typeparamref name="TDest"/> object according to the specified <paramref name="sortingParams"/> parameter.
        /// </summary>
        /// <param name="sortingParams">The ordering used to sort projected <typeparamref name="TDest"/> objects.</param>
        public ProjectFirstQuery(params OrderCreteria<TDest>[] sortingParams) : base(sortingParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <<see cref="ProjectFirstQuery{TSource, TDest}"/> class.
        /// Returns first <typeparamref name="TDest"/> object according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// Includes only those <typeparamref name="TDest"/> members that was specified in <paramref name="fetchStrategy"/> parameter.
        /// </summary>
        /// <param name="fetchStrategy">The members of <typeparamref name="TDest"/> to populate.</param>
        /// <param name="sortingParams">The ordering used to sort <typeparamref name="TDest"/> objects.</param>
        public ProjectFirstQuery(IFetchStrategy<TDest> fetchStrategy, params OrderCreteria<TDest>[] sortingParams) : base(fetchStrategy, sortingParams)
        {
        }

        #endregion

        #region Filter use cases

        /// <summary>
        /// Initializes a new instance of the <<see cref="ProjectFirstQuery{TSource, TDest}"/> class.
        /// Returns first <typeparamref name="TDest"/> object that satisfied the <paramref name="specification"/> creteria.
        /// </summary>
        /// <param name="specification">The creteria used to filter <typeparamref name="TDest"/> objects.</param>
        public ProjectFirstQuery(ISpecification<TDest> specification) : base(specification)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <<see cref="ProjectFirstQuery{TSource, TDest}"/> class.
        /// Returns first <typeparamref name="TDest"/> object that satisfied the <paramref name="specification"/> creteria according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// </summary>
        /// <param name="specification">The creteria used to filter <typeparamref name="TDest"/> objects.</param>
        /// <param name="sortingParams">The ordering used to sort <typeparamref name="TDest"/> objects.</param>
        public ProjectFirstQuery(ISpecification<TDest> specification, params OrderCreteria<TDest>[] sortingParams) : base(specification, sortingParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <<see cref="ProjectFirstQuery{TSource, TDest}"/> class.
        /// Returns first <typeparamref name="TDest"/> object that satisfied the <paramref name="specification"/> creteria according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// Includes only those <typeparamref name="TDest"/> members that was specified in <paramref name="fetchStrategy"/> parameter.
        /// </summary>
        /// <param name="fetchStrategy">The members of <typeparamref name="TDest"/> to populate.</param>
        /// <param name="specification">The creteria used to filter <typeparamref name="TDest"/> objects.</param>
        /// <param name="sortingParams">The ordering used to sort <typeparamref name="TDest"/> objects.</param>
        public ProjectFirstQuery(IFetchStrategy<TDest> fetchStrategy, ISpecification<TDest> specification, params OrderCreteria<TDest>[] sortingParams) : base(fetchStrategy, specification, sortingParams)
        {
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <<see cref="ProjectFirstQuery{TSource, TDest}"/> class.
        /// Returns first <typeparamref name="TDest"/> object that satisfied the <paramref name="specification"/> creteria according to the order specified in the <paramref name="sortingParams"/> parameter.
        /// Includes only those <typeparamref name="TDest"/> members that was specified in <paramref name="fetchStrategy"/> parameter.
        /// </summary>
        /// <param name="fetchStrategy">The members of <typeparamref name="TDest"/> to populate.</param>
        /// <param name="specification">The creteria used to filter <typeparamref name="TDest"/> objects.</param>
        /// <param name="sortingParams">The ordering used to sort <typeparamref name="TDest"/> objects.</param>
        public ProjectFirstQuery(IFetchStrategy<TDest> fetchStrategy, ISpecification<TDest> specification, IEnumerable<OrderCreteria<TDest>> orderBy) : base(fetchStrategy, specification, orderBy)
        {
        }
    }
}

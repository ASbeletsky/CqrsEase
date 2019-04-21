namespace Cqrs.Common.Queries
{
    #region Using
    using Cqrs.Core.Abstractions;
    using NSpecifications;
    #endregion

    /// <summary>
    /// Represents a query for count objects that matches specified creteria.
    /// </summary>
    /// <typeparam name="T">The type of data objects to count.</typeparam>
    public class CountQuery<T> : IQuery<long>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountQuery{T}"/> class.
        /// </summary>
        /// <param name="specification"></param>
        public CountQuery(ISpecification<T> specification)
        {
            Specification = specification;
        }

        /// <summary>
        /// The creteria used to search <typeparamref name="T"/> objects.
        /// </summary>
        public ISpecification<T> Specification { get; }
    }
}

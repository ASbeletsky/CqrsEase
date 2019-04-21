namespace Cqrs.Common.Queries
{
    #region Using
    using Cqrs.Core.Abstractions;
    using NSpecifications;
    #endregion

    /// <summary>
    /// Represents a query for verifying whether system contains objects that matches specified creteria.
    /// </summary>
    /// <typeparam name="T">The type of data objects to verify.</typeparam>
    public class ExistsQuery<T> : IQuery<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExistsQuery{T}"/> class.
        /// </summary>
        /// <param name="specification">The creteria used to search <typeparamref name="T"/> objects.</param>
        public ExistsQuery(ISpecification<T> specification)
        {
            Specification = specification;
        }

        /// <summary>
        /// The creteria used to search <typeparamref name="T"/> objects.
        /// </summary>
        public ISpecification<T> Specification { get; }
    }
}

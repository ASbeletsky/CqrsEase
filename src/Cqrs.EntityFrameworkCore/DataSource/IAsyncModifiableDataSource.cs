namespace Cqrs.EntityFrameworkCore.DataSource
{
    using NSpecifications;
    using System.Threading.Tasks;

    public interface IAsyncModifiableDataSource
    {
        Task<T> Create<T>(T value);

        Task<int> Update<T>(ISpecification<T> applyTo, T value);

        Task<int> Delete<T>(ISpecification<T> applyTo);
    }
}

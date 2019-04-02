namespace Cqrs.EntityFrameworkCore.DataSource
{
    using NSpecifications;
    using System.Threading.Tasks;

    public interface IAsyncModifiableDataSource
    {
        Task<T> CreateAsync<T>(T value) where T : class;

        Task<int> UpdateAsync<T>(ISpecification<T> applyTo, T value) where T : class;

        Task<int> DeleteAsync<T>(ISpecification<T> applyTo) where T : class;
    }
}

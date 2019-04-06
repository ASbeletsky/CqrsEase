namespace Cqrs.EntityFrameworkCore.DataSource
{
    using NSpecifications;
    using System.Threading.Tasks;

    public interface IAsyncModifiableDataSource
    {
        Task<T> CreateAsync<T>(T value) where T : class;

        Task<bool> UpdateFirstAsync<T>(ISpecification<T> applyTo, T value) where T : class;

        Task<int> UpdateRangeAsync<T>(ISpecification<T> applyTo, T value) where T : class;

        Task<bool> DeleteFirstAsync<T>(ISpecification<T> applyTo) where T : class;

        Task<int> DeleteRangeAsync<T>(ISpecification<T> applyTo) where T : class;
    }
}

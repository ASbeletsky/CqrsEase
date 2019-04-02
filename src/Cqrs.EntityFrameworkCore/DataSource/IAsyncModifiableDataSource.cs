namespace Cqrs.EntityFrameworkCore.DataSource
{
    using NSpecifications;
    using System.Threading.Tasks;

    public interface IAsyncModifiableDataSource
    {
        Task<T> Create<T>(T value) where T : class;

        Task<int> Update<T>(ISpecification<T> applyTo, T value) where T : class;

        Task<int> Delete<T>(ISpecification<T> applyTo) where T : class;
    }
}

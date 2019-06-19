namespace CqrsEase.EntityFrameworkCore.DataSource
{
    using NSpecifications;
    using System.Threading.Tasks;

    public interface IPartiallyModifiableDataSourceAsync
    {
        Task<bool> UpdateFirstAsync<T>(ISpecification<T> applyTo, object assignableValue) where T : class;

        Task<int> UpdateRangeAsync<T>(ISpecification<T> applyTo, object assignableValue) where T : class;
    }
}

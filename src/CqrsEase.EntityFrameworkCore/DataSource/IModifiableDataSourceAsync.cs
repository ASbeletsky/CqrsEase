namespace CqrsEase.EntityFrameworkCore.DataSource
{
    #region Using
    using NSpecifications;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    #endregion

    public interface IModifiableDataSourceAsync
    {
        Task<T> CreateAsync<T>(T value) where T : class;

        Task CreateRangeAsync<T>(IEnumerable<T> values) where T : class;

        Task<bool> UpdateFirstAsync<T>(ISpecification<T> applyTo, T value) where T : class;
    
        Task<int> UpdateRangeAsync<T>(ISpecification<T> applyTo, T value) where T : class;

        Task<bool> DeleteFirstAsync<T>(ISpecification<T> applyTo) where T : class;

        Task<int> DeleteRangeAsync<T>(ISpecification<T> applyTo) where T : class;
    }
}

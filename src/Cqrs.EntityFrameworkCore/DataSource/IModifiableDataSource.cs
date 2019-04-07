namespace Cqrs.EntityFrameworkCore.DataSource
{
    #region Using
    using NSpecifications;
    using System.Collections.Generic;
    #endregion

    public interface IModifiableDataSource
    {
        T Create<T>(T value) where T : class;

        void CreateRange<T>(IEnumerable<T> values) where T : class;

        bool UpdateFirst<T>(ISpecification<T> applyTo, T value) where T : class;

        int UpdateRange<T>(ISpecification<T> applyTo, T value) where T : class;

        bool DeleteFirst<T>(ISpecification<T> applyTo) where T : class;

        int DeleteRange<T>(ISpecification<T> applyTo) where T : class;
    }
}

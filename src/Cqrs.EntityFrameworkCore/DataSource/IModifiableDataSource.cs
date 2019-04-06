namespace Cqrs.EntityFrameworkCore.DataSource
{
    using NSpecifications;

    public interface IModifiableDataSource
    {
        T Create<T>(T value) where T : class;

        bool UpdateFirst<T>(ISpecification<T> applyTo, T value) where T : class;

        int UpdateRange<T>(ISpecification<T> applyTo, T value) where T : class;

        bool DeleteFirst<T>(ISpecification<T> applyTo) where T : class;

        int DeleteRange<T>(ISpecification<T> applyTo) where T : class;
    }
}

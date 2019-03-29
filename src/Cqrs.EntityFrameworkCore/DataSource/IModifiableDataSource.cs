namespace Cqrs.EntityFrameworkCore.DataSource
{
    using NSpecifications;

    public interface IModifiableDataSource
    {
        T Create<T>(T value) where T : class;

        int Update<T>(ISpecification<T> applyTo, T value) where T : class;

        int Delete<T>(ISpecification<T> applyTo) where T : class;
    }
}

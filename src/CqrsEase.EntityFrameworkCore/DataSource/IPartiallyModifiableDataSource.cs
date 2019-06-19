namespace CqrsEase.EntityFrameworkCore.DataSource
{
    using NSpecifications;

    public interface IPartiallyModifiableDataSource
    {
        bool UpdateFirst<T>(ISpecification<T> applyTo, object assignableValue) where T : class;

        int UpdateRange<T>(ISpecification<T> applyTo, object assignableValue) where T : class;
    }
}

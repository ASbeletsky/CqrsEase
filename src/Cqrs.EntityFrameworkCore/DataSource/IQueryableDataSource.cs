namespace Cqrs.EntityFrameworkCore.DataSource
{

    using System.Linq;

    interface IQueryableDataSource
    {
        IQueryable<T> Query<T>() where T : class;
    }
}

using System.Collections.Generic;
using System.Linq;

namespace Cqrs.EntityFrameworkCore
{
    public interface IProjector
    {
        IQueryable<TDest> ProjectTo<TDest>(IQueryable queryable);
        IQueryable<T> ProjectOnly<T>(IQueryable<T> queryable, IEnumerable<string> paths);
        IQueryable<TDest> ProjectOnly<TSource, TDest>(IQueryable<TSource> queryable, IEnumerable<string> paths);
    }
}

using System.Collections.Generic;
using System.Linq;

namespace Cqrs.EntityFrameworkCore
{
    public interface IProjector
    {
        IQueryable<TDest> ProjectTo<TDest>(IQueryable queryable);

        IQueryable<TDest> ProjectOnly<TSource, TDest>(IQueryable<TSource> queryable, IEnumerable<string> paths);
    }
}

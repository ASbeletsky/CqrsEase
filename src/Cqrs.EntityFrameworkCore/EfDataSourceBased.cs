using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cqrs.EntityFrameworkCore
{
    public class EfDataSourceBased
    {
        internal readonly DbContext _dbContext;

        public EfDataSourceBased(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>();
        }
    }
}

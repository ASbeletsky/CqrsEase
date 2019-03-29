using Microsoft.EntityFrameworkCore;
using NSpecifications;
using System.Linq;

namespace Cqrs.EntityFrameworkCore.DataSource
{
    public class EfDataSourceBased : IQueryableDataSource, IModifiableDataSource
    {
        internal readonly DbContext _dbContext;

        public EfDataSourceBased(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        public T Create<T>(T value) where T : class
        {
            _dbContext.Add<T>(value);        
            _dbContext.SaveChanges();
            return value;
        }

        public int Update<T>(ISpecification<T> applyTo, T value) where T : class
        {
            int updatedCount = 0;
            var entitiesToUpdate = Query<T>().MaybeWhere(applyTo).ToList();
            if(entitiesToUpdate != null && entitiesToUpdate.Any())
            {
                throw new System.NotImplementedException();
            }

            return updatedCount;
        }

        public int Delete<T>(ISpecification<T> applyTo) where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}

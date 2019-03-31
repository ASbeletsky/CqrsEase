using Microsoft.EntityFrameworkCore;
using NSpecifications;
using System.Linq;
using System.Reflection;

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
                foreach (var entity in entitiesToUpdate)
                {
                    SetValues(entity, value);
                    updatedCount++;
                }

                _dbContext.SaveChanges();
            }

            return updatedCount;
        }

        private void SetValues<T>(T from, T to) where T : class
        {
            var entityType = from.GetType();
            var entry = _dbContext.Entry(from);
            var propertiesToSet = _dbContext.Model.FindEntityType(entityType)
                .GetProperties()
                .Where(p => !p.IsPrimaryKey())
                .ToDictionary(p => p.Name, p => entityType.GetProperty(p.Name).GetValue(to));

            entry.CurrentValues.SetValues(values: propertiesToSet);
        }

        public int Delete<T>(ISpecification<T> applyTo) where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}

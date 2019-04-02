namespace Cqrs.EntityFrameworkCore.DataSource
{
    #region Using
    using Microsoft.EntityFrameworkCore;
    using NSpecifications;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    #endregion

    public class EfDataSourceBased : IQueryableDataSource, IModifiableDataSource, IAsyncModifiableDataSource
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

        #region IModifiableDataSource members

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
            int deletedCount = 0;
            var entitiesToDelete = Query<T>().MaybeWhere(applyTo).ToList();
            if (entitiesToDelete != null && entitiesToDelete.Any())
            {
                foreach (var entity in entitiesToDelete)
                {
                    _dbContext.Set<T>().Remove(entity);
                    deletedCount++;
                }

                _dbContext.SaveChanges();
            }

            return deletedCount;
        }

        #endregion

        #region IAsyncModifiableDataSource members

        public async Task<T> CreateAsync<T>(T value) where T : class
        {
            _dbContext.Add<T>(value);
            await _dbContext.SaveChangesAsync();
            return value;
        }

        public async Task<int> UpdateAsync<T>(ISpecification<T> applyTo, T value) where T : class
        {
            int updatedCount = 0;
            var entitiesToUpdate = await Query<T>().MaybeWhere(applyTo).ToListAsync();
            if (entitiesToUpdate != null && entitiesToUpdate.Any())
            {
                foreach (var entity in entitiesToUpdate)
                {
                    SetValues(entity, value);
                    updatedCount++;
                }

                await _dbContext.SaveChangesAsync();
            }

            return updatedCount;
        }

        public async Task<int> DeleteAsync<T>(ISpecification<T> applyTo) where T : class
        {
            int deletedCount = 0;
            var entitiesToDelete = await Query<T>().MaybeWhere(applyTo).ToListAsync();
            if (entitiesToDelete != null && entitiesToDelete.Any())
            {
                foreach (var entity in entitiesToDelete)
                {
                    _dbContext.Set<T>().Remove(entity);
                    deletedCount++;
                }

                await _dbContext.SaveChangesAsync();
            }

            return deletedCount;
        }

        #endregion
    }
}

namespace Cqrs.EntityFrameworkCore.DataSource
{
    #region Using
    using Microsoft.EntityFrameworkCore;
    using NSpecifications;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    #endregion

    public class EfDataSourceBased
        : IQueryableDataSource
        , IModifiableDataSource
        , IPartiallyModifiableDataSource
        , IModifiableDataSourceAsync
        , IPartiallyModifiableDataSourceAsync
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

        public void CreateRange<T>(IEnumerable<T> values) where T : class
        {
            _dbContext.AddRange(values);
            _dbContext.SaveChanges();
        }

        public bool UpdateFirst<T>(ISpecification<T> applyTo, T value) where T : class
        {
            return this.UpdateFirst<T>(applyTo, assignableValue: value);
        }

        public int UpdateRange<T>(ISpecification<T> applyTo, T value) where T : class
        {
            return this.UpdateRange<T>(applyTo, assignableValue: value);
        }    

        public bool DeleteFirst<T>(ISpecification<T> applyTo) where T : class
        {
            bool isDeleted = false;
            var entityToUpdate = Query<T>().MaybeWhere(applyTo).FirstOrDefault();
            if(entityToUpdate != null)
            {
                _dbContext.Entry(entityToUpdate).State = EntityState.Deleted;
                _dbContext.SaveChanges();
                isDeleted = true;
            }

            return isDeleted;
        }

        public int DeleteRange<T>(ISpecification<T> applyTo) where T : class
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

        #region IPartiallyModifiableDataSource members

        private void SetValues<T>(object from, T to)
            where T : class
        {
            var destType = from.GetType();
            var sourceType = to.GetType();
            var entry = _dbContext.Entry(from);
            var propertiesToSet = _dbContext.Model.FindEntityType(destType)
                .GetProperties()
                .Where(p => !p.IsPrimaryKey())
                .ToDictionary(p => p.Name, p => sourceType.GetProperty(p.Name).GetValue(to));

            entry.CurrentValues.SetValues(values: propertiesToSet);
        }

        public bool UpdateFirst<T>(ISpecification<T> applyTo, object assignableValue) where T : class
        {
            bool isUpdated = false;
            var entityToUpdate = Query<T>().MaybeWhere(applyTo).FirstOrDefault();
            if (entityToUpdate != null)
            {
                SetValues(entityToUpdate, assignableValue);
                isUpdated = true;
                _dbContext.SaveChanges();
            }

            return isUpdated;
        }
        public int UpdateRange<T>(ISpecification<T> applyTo, object assignableValue) where T : class
        {
            int updatedCount = 0;
            var entitiesToUpdate = Query<T>().MaybeWhere(applyTo).ToList();
            if (entitiesToUpdate != null && entitiesToUpdate.Any())
            {
                foreach (var entity in entitiesToUpdate)
                {
                    SetValues(entity, assignableValue);
                    updatedCount++;
                }

                _dbContext.SaveChanges();
            }

            return updatedCount;
        }

        #endregion

        #region IModifiableDataSourceAsync members

        public async Task<T> CreateAsync<T>(T value) where T : class
        {
            _dbContext.Add<T>(value);
            await _dbContext.SaveChangesAsync();
            return value;
        }

        public async Task CreateRangeAsync<T>(IEnumerable<T> values) where T : class
        {
            _dbContext.AddRange(values);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateFirstAsync<T>(ISpecification<T> applyTo, T value) where T : class
        {
            return await this.UpdateFirstAsync<T>(applyTo, assignableValue: value);
        }

        public async Task<int> UpdateRangeAsync<T>(ISpecification<T> applyTo, T value) where T : class
        {
            return await this.UpdateRangeAsync<T>(applyTo, assignableValue: value);
        }

        public async Task<bool> DeleteFirstAsync<T>(ISpecification<T> applyTo) where T : class
        {
            bool isDeleted = false;
            var entityToUpdate = await Query<T>().MaybeWhere(applyTo).FirstOrDefaultAsync();
            if (entityToUpdate != null)
            {
                _dbContext.Entry(entityToUpdate).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
                isDeleted = true;
            }

            return isDeleted;
        }

        public async Task<int> DeleteRangeAsync<T>(ISpecification<T> applyTo) where T : class
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

        #region IPartiallyModifiableDataSourceAsync members

        public async Task<bool> UpdateFirstAsync<T>(ISpecification<T> applyTo, object assignableValue) where T : class
        {
            bool isUpdated = false;
            var entityToUpdate = await Query<T>().MaybeWhere(applyTo).FirstOrDefaultAsync();
            if (entityToUpdate != null)
            {
                SetValues(entityToUpdate, assignableValue);
                isUpdated = true;
                await _dbContext.SaveChangesAsync();
            }

            return isUpdated;
        }

        public async Task<int> UpdateRangeAsync<T>(ISpecification<T> applyTo, object assignableValue) where T : class
        {
            int updatedCount = 0;
            var entitiesToUpdate = await Query<T>().MaybeWhere(applyTo).ToListAsync();
            if (entitiesToUpdate != null && entitiesToUpdate.Any())
            {
                foreach (var entity in entitiesToUpdate)
                {
                    SetValues(entity, assignableValue);
                    updatedCount++;
                }

                await _dbContext.SaveChangesAsync();
            }

            return updatedCount;
        }

        #endregion
    }
}

namespace CqrsEase.EntityFrameworkCore.DataSource
{
    #region Using
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    #endregion

    public class DataSourceFactory
    {
        private readonly ConcurrentBag<Type> _dbContexts;
        private readonly IServiceProvider _serviceProvider;

        public DataSourceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dbContexts = new ConcurrentBag<Type>();
        }

        public void RegisterDatasource<TDbContext>() where TDbContext : DbContext
        {
            var dbContextType = typeof(TDbContext);
            _dbContexts.Add(dbContextType);
        }

        public EfDataSourceBased GetForEntity<TEntity>()
        {
            var entityType = typeof(TEntity);
            foreach (var dbContextType in _dbContexts)
            {
                var dbContext = (DbContext)_serviceProvider.GetService(dbContextType);
                if (dbContext.Model.FindEntityType(entityType) != null)
                {
                    return new EfDataSourceBased(dbContext);
                }
            }

            string dbContextsTypes = string.Join(", ", _dbContexts.Select(c => c.Name));
            throw new ArgumentException($"The type {entityType.Name} doesn't belogs to any provided dbContexts: {dbContextsTypes}");
        }
    }
}

namespace Cqrs.EntityFrameworkCore.FetchStrategies
{
    #region Using
    using Cqrs.Common.Queries.FetchStrategies;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    #endregion

    public class FetchAllExceptNavigationsStrategy<T> : FetchAllExceptStrategy<T>
    {
        public FetchAllExceptNavigationsStrategy(DbContext dbContext)
            : base(dbContext.Model.FindEntityType(typeof(T))
                    ?.GetNavigations().Select(n => n.Name).ToArray()
                    ?? throw new ArgumentException($"The type {typeof(T).Name} is not entity type of {dbContext.GetType().Name} dbContext")
            )
        {
        }
    }
}

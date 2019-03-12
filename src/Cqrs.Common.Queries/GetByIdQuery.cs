namespace Cqrs.Common.Queries
{
    using Cqrs.Core.Abstractions;
    using Cqrs.Model.Abstractions;
    
    public class GetByIdQuery<TKey, TEntity> : IQuery<TEntity>
            where TEntity : class, Identifiable<TKey>
    {
        public TKey Id { get; private set; }

        public IFetchStrategy<TEntity> FetchStrategy { get; set; }

        public GetByIdQuery(TKey id) : this(id, null)
        {

        }
        
        public GetByIdQuery(TKey id, IFetchStrategy<TEntity> fetchStrategy)
        {
            Id = id;
            FetchStrategy = fetchStrategy;            
        }
    }
}

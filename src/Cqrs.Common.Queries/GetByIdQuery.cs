namespace Cqrs.Common.Queries
{
    using Cqrs.Core.Abstractions;
    using Cqrs.Model.Abstractions;

    public class GetByIdQuery<TKey, TEntity> : IQuery<TEntity>
            where TEntity : class, Identifiable<TKey>
    {
        public TKey Id { get; private set; }

        public GetByIdQuery(TKey id)
        {
            Id = id;
        }
    }
}

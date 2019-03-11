namespace Cqrs.Common.Commands
{
    using Cqrs.Core.Abstractions;
    using Cqrs.Model.Abstractions;

    public abstract class EntityCommand<TEntity> : ICommand
        where TEntity : Identifiable
    {
        protected EntityCommand(TEntity entity)
        {
            Entity = entity;
        }

        public TEntity Entity { get; }
    }

    public abstract class EntityCommand<TKey, TEntity> : EntityCommand<TEntity>, ICommand<TKey>
        where TEntity : Identifiable<TKey>
    {
        protected EntityCommand(TEntity entity) : base(entity)
        {
        }
    }
}

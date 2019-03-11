namespace Cqrs.Common.Commands
{
    using Cqrs.Core.Abstractions;
    using Cqrs.Model.Abstractions;

    public class DeleteEntityCommand<TEntity> : EntityCommand<TEntity>
            where TEntity : Identifiable
    {
        public DeleteEntityCommand(TEntity entity) : base(entity)
        {
        }
    }

    public class DeleteByIdCommand<TEntity, TKey> : ICommand
        where TEntity : Identifiable<TKey>
    {
        public DeleteByIdCommand(TKey id)
        {
            Id = id;
        }

        public TKey Id { get; }
    }
}

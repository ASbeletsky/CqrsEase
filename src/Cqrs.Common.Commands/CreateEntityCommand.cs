namespace Cqrs.Common.Commands
{
    using Cqrs.Model.Abstractions;

    public class CreateEntityCommand<TKey, TEntity> : EntityCommand<TKey, TEntity>
            where TEntity : Identifiable<TKey>
    {
        public CreateEntityCommand(TEntity entity) : base(entity)
        {
        }
    }
}

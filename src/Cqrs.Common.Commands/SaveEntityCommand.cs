using Cqrs.Model.Abstractions;

namespace Cqrs.Common.Commands
{
    public class SaveEntityCommand<TKey, TEntity> : EntityCommand<TKey, TEntity>
        where TEntity : Identifiable<TKey>
    {
        public SaveEntityCommand(TEntity entity) : base(entity)
        {
        }
    }
}

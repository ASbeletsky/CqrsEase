namespace Cqrs.Common.Commands
{
    using Cqrs.Model.Abstractions;

    public class UpdateEntityCommand<TEntity> : EntityCommand<TEntity>
        where TEntity : Identifiable
    {
        public UpdateEntityCommand(TEntity entity) : base(entity)
        {
        }
    }
}

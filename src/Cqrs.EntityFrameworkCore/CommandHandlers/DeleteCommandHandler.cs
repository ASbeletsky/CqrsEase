namespace Cqrs.EntityFrameworkCore.CommandHandlers
{
    #region Using
    using Cqrs.Common.Commands;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore.DataSource;
    using System.Threading.Tasks;
    #endregion

    public class DeleteCommandHandler<T>
        : ICommandHandler<DeleteCommand<T>, IDeleteResult>
        , ICommandHandlerAsync<DeleteCommand<T>, IDeleteResult>
        where T : class
    {
        public DeleteCommandHandler(EfDataSourceBased dataSource)
        {
            DataSource = dataSource;
        }

        public DeleteCommandHandler(DataSourceFactory dataSourceFactory)
            : this(dataSourceFactory.GetForEntity<T>())
        {
        }

        public EfDataSourceBased DataSource { get; }

        public IDeleteResult Apply(DeleteCommand<T> command)
        {
            int deletedCount;
            if (command.DeleteFirstMatchOnly)
            {
                deletedCount = DataSource.DeleteFirst(command.ApplyTo) ? 1 : 0;
            }
            else
            {
                deletedCount = DataSource.DeleteRange(command.ApplyTo);
            }

            return new DeleteResult(deletedCount);
        }

        public async Task<IDeleteResult> ApplyAsync(DeleteCommand<T> command)
        {
            int deletedCount;
            if (command.DeleteFirstMatchOnly)
            {
                deletedCount = await DataSource.DeleteFirstAsync(command.ApplyTo) ? 1 : 0;
            }
            else
            {
                deletedCount = await DataSource.DeleteRangeAsync(command.ApplyTo);
            }

            return new DeleteResult(deletedCount);
        }
    }
}

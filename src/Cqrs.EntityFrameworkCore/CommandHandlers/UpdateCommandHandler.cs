namespace Cqrs.EntityFrameworkCore.CommandHandlers
{
    #region Using
    using Cqrs.Common.Commands;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore.DataSource;
    using System.Threading.Tasks;
    #endregion

    public class UpdateCommandHandler<T>
        : ICommandHandler<UpdateCommand<T>, IUpdateResult>
        , ICommandHandlerAsync<UpdateCommand<T>, IUpdateResult>
        where T : class
    {
        public UpdateCommandHandler(EfDataSourceBased dataSource)
        {
            DataSource = dataSource;
        }

        public UpdateCommandHandler(DataSourceFactory dataSourceFactory)
            : this(dataSourceFactory.GetForEntity<T>())
        {
        }

        public EfDataSourceBased DataSource { get; }

        public IUpdateResult Apply(UpdateCommand<T> command)
        {
            int updatedCount;
            if (command.UpdateFirstMatchOnly)
            {
                updatedCount = DataSource.UpdateFirst(command.ApplyTo, command.Value) ? 1 : 0;
            }
            else
            {
                updatedCount = DataSource.UpdateRange(command.ApplyTo, command.Value);
            }

            return new UpdateResult(updatedCount);
        }

        public async Task<IUpdateResult> ApplyAsync(UpdateCommand<T> command)
        {
            int updatedCount;
            if (command.UpdateFirstMatchOnly)
            {
                updatedCount = await DataSource.UpdateFirstAsync(command.ApplyTo, command.Value) ? 1 : 0;
            }
            else
            {
                updatedCount = await DataSource.UpdateRangeAsync(command.ApplyTo, command.Value);
            }

            return new UpdateResult(updatedCount);
        }
    }
}

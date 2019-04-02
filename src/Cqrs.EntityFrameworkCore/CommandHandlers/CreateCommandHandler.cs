namespace Cqrs.EntityFrameworkCore.CommandHandlers
{
    #region Using
    using Cqrs.Common.Commands;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore.DataSource;
    using System.Threading.Tasks;
    #endregion

    public class CreateCommandHandler<T>
        : ICommandHandler<CreateCommand<T>, ICreateResult<T>>
        , ICommandHandlerAsync<CreateCommand<T>, ICreateResult<T>>
        where T : class
    {
        public CreateCommandHandler(EfDataSourceBased dataSource)
        {
            DataSource = dataSource;
        }

        public CreateCommandHandler(DataSourceFactory dataSourceFactory)
            : this(dataSourceFactory.GetForEntity<T>())
        {
        }

        internal EfDataSourceBased DataSource { get; }

        public ICreateResult<T> Apply(CreateCommand<T> command)
        {
            var createdValue = DataSource.Create<T>(command.Value);
            return new CreateResult<T>(createdValue);
        }

        public async Task<ICreateResult<T>> ApplyAsync(CreateCommand<T> command)
        {
            var createdValue = await DataSource.CreateAsync<T>(command.Value);
            return new CreateResult<T>(createdValue);
        }
    }
}

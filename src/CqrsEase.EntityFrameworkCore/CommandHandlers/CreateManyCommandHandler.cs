namespace CqrsEase.EntityFrameworkCore.CommandHandlers
{
    #region Using
    using CqrsEase.Common.Commands;
    using CqrsEase.Core.Abstractions;
    using CqrsEase.EntityFrameworkCore.DataSource;
    using System.Threading.Tasks;
    #endregion

    public class CreateManyCommandHandler<T>
        : ICommandHandler<CreateManyCommand<T>>
        , ICommandHandlerAsync<CreateManyCommand<T>>
        where T : class
    {
        public CreateManyCommandHandler(EfDataSourceBased dataSource)
        {
            DataSource = dataSource;
        }

        public CreateManyCommandHandler(DataSourceFactory dataSourceFactory)
            : this(dataSourceFactory.GetForEntity<T>())
        {
        }

        public EfDataSourceBased DataSource { get; }

        public void Apply(CreateManyCommand<T> command)
        {
            DataSource.CreateRange(command.Values);
        }

        public async Task ApplyAsync(CreateManyCommand<T> command)
        {
            await DataSource.CreateRangeAsync(command.Values);
        }
    }
}

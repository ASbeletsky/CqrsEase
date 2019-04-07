namespace Cqrs.EntityFrameworkCore.CommandHandlers
{    
    #region Using
    using Cqrs.Common.Commands;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore.DataSource;
    using AutoMapper;
    using System.Threading.Tasks;
    using System;
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

    public class UpdateCommandHandler<TDest, TSource>
        : ICommandHandler<UpdateCommand<TDest, TSource>, IUpdateResult>
        , ICommandHandlerAsync<UpdateCommand<TDest, TSource>, IUpdateResult>
        where TDest : class
        where TSource : class
    {
        public UpdateCommandHandler(EfDataSourceBased dataSource, IMapper mapper)
        {
            DataSource = dataSource;
            Mapper = mapper;
        }

        public UpdateCommandHandler(DataSourceFactory dataSourceFactory, IMapper mapper)
            : this(dataSourceFactory.GetForEntity<TDest>(), mapper)
        {
        }

        public EfDataSourceBased DataSource { get; }
        public IMapper Mapper { get; }

        protected object GetUpdatedValue(TSource sourceValue)
        {
            object value;
            if (sourceValue is TDest || sourceValue.GetType().IsAnonymous())
            {
                value = sourceValue;
            }
            else
            {
                try
                {
                    value = Mapper.Map<TSource, TDest>(sourceValue);
                }
                catch(AutoMapperConfigurationException e)
                {
                    throw new ArgumentException($"Cannot map values from {typeof(TSource).Name} type to {typeof(TDest).Name}. See inner exception for details.", e);
                }
            }

            return value;
        }

        public IUpdateResult Apply(UpdateCommand<TDest, TSource> command)
        {
            int updatedCount;
            var value = GetUpdatedValue(command.Value);
            if (command.UpdateFirstMatchOnly)
            {
                updatedCount = DataSource.UpdateFirst(command.ApplyTo, value) ? 1 : 0;
            }
            else
            {
                updatedCount = DataSource.UpdateRange(command.ApplyTo, value);
            }

            return new UpdateResult(updatedCount);
        }

        public async Task<IUpdateResult> ApplyAsync(UpdateCommand<TDest, TSource> command)
        {
            int updatedCount;
            var value = GetUpdatedValue(command.Value);
            if (command.UpdateFirstMatchOnly)
            {
                updatedCount = await DataSource.UpdateFirstAsync(command.ApplyTo, value) ? 1 : 0;
            }
            else
            {
                updatedCount = await DataSource.UpdateRangeAsync(command.ApplyTo, value);
            }

            return new UpdateResult(updatedCount);
        }
    }

}

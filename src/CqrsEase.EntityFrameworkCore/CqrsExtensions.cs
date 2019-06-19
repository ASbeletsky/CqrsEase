namespace CqrsEase.EntityFrameworkCore
{
    #region Using
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using AutoMapper;
    using CqrsEase.Common.Queries;
    using CqrsEase.Common.Queries.FetchStrategies;
    using CqrsEase.Core;
    using CqrsEase.Core.Abstractions;
    using CqrsEase.EntityFrameworkCore.DataSource;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Reflection;
    #endregion

    public static class CqrsEaseExtensions 
    {
        public static IServiceCollection UseCqrsEntityFramework<TDbContext>(this IServiceCollection services,
            Action<ContainerBuilder> registrations = null,
            Profile automapperProfile = null,
            Type defaultFetchStrategy = null)
            where TDbContext : DbContext
        {
            var defaultServiceProvider = services.BuildServiceProvider();
            var factory = defaultServiceProvider.GetService<DataSourceFactory>() ?? new DataSourceFactory(defaultServiceProvider);
            services.AddSingleton(typeof(DataSourceFactory), factory);
            factory.RegisterDatasource<TDbContext>();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule<EfCoreHandlers>();
            registrations?.Invoke(builder);
            var mapperConfig = new MapperConfiguration(c =>
            {
                if (automapperProfile != null)
                {
                    c.AddProfile(automapperProfile);
                }
            });

            builder.Register(c => mapperConfig).As<IConfigurationProvider>().SingleInstance();
            builder.Register(ctx =>
            {
                 var scope = ctx.Resolve<ILifetimeScope>();
                 return new Mapper(ctx.Resolve<IConfigurationProvider>(), scope.Resolve);
            }).As<IMapper>().InstancePerLifetimeScope();

            if (defaultFetchStrategy != null && typeof(IFetchStrategy<>).GetTypeInfo().IsAssignableFrom(defaultFetchStrategy.GetTypeInfo()))
            {
                throw new ArgumentException("Default fetch strategy type doesn't implement IFetchStrategy interface", nameof(defaultFetchStrategy));
            }
            else
            {
                defaultFetchStrategy = typeof(FetchAllStrategy<>);
            }

            builder.RegisterGeneric(defaultFetchStrategy).As(typeof(IFetchStrategy<>));
            var serviceProvider = new AutofacServiceProvider(builder.Build());
            ServiceProvider.Initialize(serviceProvider);
            services.AddSingleton<AutofacServiceProvider>(serviceProvider);
            services.AddScoped<IQueryProcessor, QueryProcessor>(x => new QueryProcessor(x.GetService<AutofacServiceProvider>()));
            return services;
        }
    }
}

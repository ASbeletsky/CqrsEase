namespace Cqrs.EntityFrameworkCore
{
    #region Using
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using AutoMapper;
    using Cqrs.Core;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore.DataSource;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    #endregion

    public static class CqrsExtensions 
    {
        public static IServiceCollection UseCqrsEntityFramework<TDbContext>(this IServiceCollection services, Action<ContainerBuilder> registrations = null, Profile automapperProfile = null) where TDbContext : DbContext
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

            builder.RegisterType<AutoMapperProjector>().As<IProjector>().InstancePerLifetimeScope();

            var serviceProvider = new AutofacServiceProvider(builder.Build());
            ServiceProvider.Initialize(serviceProvider);
            services.AddSingleton<AutofacServiceProvider>(serviceProvider);
            services.AddScoped<IQueryProcessor, QueryProcessor>(x => new QueryProcessor(x.GetService<AutofacServiceProvider>()));
            return services;
        }
    }
}

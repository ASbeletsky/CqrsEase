namespace Cqrs.EntityFrameworkCore
{
    #region Using
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Cqrs.Core;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore.DataSource;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    #endregion

    public static class CqrsExtensions 
    {
        public static IServiceCollection UseCqrsEntityFramework<TDbContext>(this IServiceCollection services, Action<ContainerBuilder> registrations = null) where TDbContext : DbContext
        {
            var defaultServiceProvider = services.BuildServiceProvider();
            var factory = defaultServiceProvider.GetService<DataSourceFactory>() ?? new DataSourceFactory(defaultServiceProvider);
            services.AddSingleton(typeof(DataSourceFactory), factory);
            factory.RegisterDatasource<TDbContext>();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule<EfCoreHandlers>();
            registrations?.Invoke(builder);

            var serviceProvider = new AutofacServiceProvider(builder.Build());
            services.AddSingleton<AutofacServiceProvider>(serviceProvider);
            services.AddScoped<IQueryProcessor, QueryProcessor>(x => new QueryProcessor(x.GetService<AutofacServiceProvider>()));
            return services;
        }
    }
}

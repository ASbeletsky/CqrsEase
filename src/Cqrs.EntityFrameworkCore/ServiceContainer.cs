using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cqrs.Core;
using Cqrs.Core.Abstractions;
using Cqrs.EntityFrameworkCore.QueryHandlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cqrs.EntityFrameworkCore
{
    public static class MicrosoftDIExtensions 
    {
        public static IServiceCollection UseCqrsEntityFramework<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterType<EfDataSourceBased>()
                .WithParameter((param, ctx) => param.Name == "DbContext", (param, ctx) => ctx.Resolve<TDbContext>())
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(GetFirstQueryHandler<>))
                .WithParameter((param, ctx) => param.Name == "dataSource", (param, ctx) => ctx.Resolve<EfDataSourceBased>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var serviceProvider = new AutofacServiceProvider(builder.Build());
            services.AddScoped<IQueryProcessor, QueryProcessor>(x => new QueryProcessor(serviceProvider));
            return services;
        }
    }
}

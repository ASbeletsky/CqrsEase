using Autofac;
using Cqrs.EntityFrameworkCore.DataSource;
using Cqrs.EntityFrameworkCore.QueryHandlers;

namespace Cqrs.EntityFrameworkCore
{
    internal class EfCoreHandlers : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<AutoMapperProjector>()
                .As<IProjector>()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(GetFirstQueryHandler<>))
                .AsImplementedInterfaces()
                .UsingConstructor(typeof(DataSourceFactory))
                .WithParameter((param, ctx) => param.Name == "dataSourceFactory", (param, ctx) => ctx.Resolve<DataSourceFactory>())
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ProjectFirstQueryHandler<,>))
                .AsImplementedInterfaces()
                .UsingConstructor(typeof(DataSourceFactory), typeof(IProjector))
                .WithParameter((param, ctx) => param.Name == "dataSourceFactory", (param, ctx) => ctx.Resolve<DataSourceFactory>())
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(GetManyQueryHandler<>))
                .AsImplementedInterfaces()
                .UsingConstructor(typeof(DataSourceFactory))
                .WithParameter((param, ctx) => param.Name == "dataSourceFactory", (param, ctx) => ctx.Resolve<DataSourceFactory>())
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ProjectManyQueryHandler<,>))
                .AsImplementedInterfaces()
                .UsingConstructor(typeof(DataSourceFactory), typeof(IProjector))
                .WithParameter((param, ctx) => param.Name == "dataSourceFactory", (param, ctx) => ctx.Resolve<DataSourceFactory>())
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ExistsQueryHandler<>))
                .AsImplementedInterfaces()
                .UsingConstructor(typeof(DataSourceFactory))
                .WithParameter((param, ctx) => param.Name == "dataSourceFactory", (param, ctx) => ctx.Resolve<DataSourceFactory>())
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(CountQueryHandler<>))
                .AsImplementedInterfaces()
                .UsingConstructor(typeof(DataSourceFactory))
                .WithParameter((param, ctx) => param.Name == "dataSourceFactory", (param, ctx) => ctx.Resolve<DataSourceFactory>())
                .InstancePerLifetimeScope();
        }
    }
}

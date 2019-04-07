namespace Cqrs.EntityFrameworkCore
{
    #region Using
    using Autofac;
    using AutoMapper;
    using Cqrs.EntityFrameworkCore.CommandHandlers;
    using Cqrs.EntityFrameworkCore.DataSource;
    using Cqrs.EntityFrameworkCore.QueryHandlers;
    using System;
    using System.Reflection;
    #endregion

    internal class EfCoreHandlers : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<AutoMapperProjector>()
                .As<IProjector>()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(GetFirstQueryHandler<>))
                .UsingConstructor(typeof(DataSourceFactory))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ProjectFirstQueryHandler<,>))
                .UsingConstructor(typeof(DataSourceFactory), typeof(IProjector))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(GetManyQueryHandler<>))
                .UsingConstructor(typeof(DataSourceFactory))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ProjectManyQueryHandler<,>))
                .UsingConstructor(typeof(DataSourceFactory), typeof(IProjector))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ExistsQueryHandler<>))
                .UsingConstructor(typeof(DataSourceFactory))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(CountQueryHandler<>))
                .UsingConstructor(typeof(DataSourceFactory))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(CreateCommandHandler<>))
                .UsingConstructor(typeof(DataSourceFactory))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(CreateManyCommandHandler<>))
                .UsingConstructor(typeof(DataSourceFactory))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(UpdateCommandHandler<>))
                .UsingConstructor(typeof(DataSourceFactory))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(UpdateCommandHandler<,>))
                .UsingConstructor(typeof(DataSourceFactory), typeof(IMapper))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(DeleteCommandHandler<>))
                .UsingConstructor(typeof(DataSourceFactory))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}

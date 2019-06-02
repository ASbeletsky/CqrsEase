namespace Cqrs.EntityFrameworkCore
{
    #region Using
    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Activators.Reflection;
    using AutoMapper;
    using Cqrs.EntityFrameworkCore.CommandHandlers;
    using Cqrs.EntityFrameworkCore.DataSource;
    using Cqrs.EntityFrameworkCore.QueryHandlers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    #endregion

    internal class ConstrustorWithDataSourceFactory : IConstructorSelector
    {
        public ConstructorParameterBinding SelectConstructorBinding(ConstructorParameterBinding[] constructorBindings, IEnumerable<Parameter> parameters)
        {
            var construstor = constructorBindings.SingleOrDefault(b => b.TargetConstructor.GetParameters().Any(p => p.ParameterType == typeof(DataSourceFactory)));
            return construstor;
        }
    }

    internal class EfCoreHandlers : Autofac.Module
    {       
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var dataSourceFactoryConstructor = new ConstrustorWithDataSourceFactory();

            builder.RegisterGeneric(typeof(GetFirstQueryHandler<>))
                .UsingConstructor(dataSourceFactoryConstructor)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(GetManyQueryHandler<>))
                .UsingConstructor(dataSourceFactoryConstructor)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            builder.RegisterGeneric(typeof(ExistsQueryHandler<>))
                .UsingConstructor(dataSourceFactoryConstructor)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(CountQueryHandler<>))
                .UsingConstructor(dataSourceFactoryConstructor)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(CreateCommandHandler<>))
                .UsingConstructor(dataSourceFactoryConstructor)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(CreateManyCommandHandler<>))
                .UsingConstructor(dataSourceFactoryConstructor)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(UpdateCommandHandler<>))
                .UsingConstructor(dataSourceFactoryConstructor)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(UpdateCommandHandler<,>))
                .UsingConstructor(dataSourceFactoryConstructor)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(DeleteCommandHandler<>))
                .UsingConstructor(dataSourceFactoryConstructor)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}

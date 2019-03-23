using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Cqrs.EntityFrameworkCore.QueryHandlers;

namespace Cqrs.EntityFrameworkCore
{
    public class ServiceContainer : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterGeneric(typeof(GetFirstQueryHandler<>)).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Cqrs.EntityFrameworkCore
{
    internal class IQueriableProjector : IProjector
    {
        public IQueriableProjector()
        {
            MapperConfig = new MapperConfiguration(x => x.CreateMissingTypeMaps = true);           
            Mapper = MapperConfig.CreateMapper();
        }

        public MapperConfiguration MapperConfig { get; }
        public IMapper Mapper { get; private set; }

        public IQueryable<TDest> ProjectOnly<TSource, TDest>(IQueryable<TSource> queryable, IEnumerable<string> paths)
        {
            return queryable.ProjectTo<TDest>(MapperConfig, parameters: null, membersToExpand: paths.ToArray());
        }

        public IQueryable<T> ProjectOnly<T>(IQueryable<T> queryable, IEnumerable<string> paths)
        {
            var projectionSelector = BuildProjectOnlySelector<T>(paths);
            return queryable.Select(projectionSelector).AsQueryable();
        }

        private Func<T, T> BuildProjectOnlySelector<T>(IEnumerable<string> paths)
        {
            var lamdaParameter = Expression.Parameter(typeof(T), "x");
            var xNew = Expression.New(typeof(T));
            var bindings = paths.Select(o =>
            {
                var pathProperty = typeof(T).GetProperty(o);
                var getPropertyExpression = Expression.Property(lamdaParameter, pathProperty);
                // set value "Field1 = x.Field1"
                return Expression.Bind(pathProperty, getPropertyExpression);
            });

            var xInit = Expression.MemberInit(xNew, bindings);
            // expression "x => new T { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<T, T>>(xInit, lamdaParameter);
            return lambda.Compile();
        }

        public IQueryable<TDest> ProjectTo<TDest>(IQueryable queryable)
        {
            return queryable.ProjectTo<TDest>();
        }
    }
}

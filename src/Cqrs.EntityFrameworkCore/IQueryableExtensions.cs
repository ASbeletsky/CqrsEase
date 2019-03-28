namespace Cqrs.EntityFrameworkCore
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Common.Queries.Sorting;
    using Microsoft.EntityFrameworkCore;
    using NSpecifications;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    #endregion

    internal static class IQueriableExtensions
    {
        internal static IQueryable<T> ApplyFetchStrategy<T>(this IQueryable<T> source, IFetchStrategy<T> fetchStrategy, DbContext dbContext)
            where T : class
        {
            if (fetchStrategy != null)
            {
                var pathsToInclude = dbContext.Model.FindEntityType(typeof(T))
                    .GetNavigations()
                    .Where(p => fetchStrategy.FetchedPaths.Contains(p.Name));

                foreach (var path in pathsToInclude)
                {
                    source = source.Include(path.Name);
                }

                return source.SelectOnly<T>(fetchStrategy.FetchedPaths);
            }

            return source;
        }

        internal static IQueryable<T> SelectOnly<T>(this IQueryable<T> queryable, IEnumerable<string> paths)
        {
            var projectionSelector = BuildSelector<T>(paths);
            return queryable.Select(projectionSelector).AsQueryable();
        }

        private static Func<T, T> BuildSelector<T>(IEnumerable<string> paths)
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

        internal static IQueryable<T> MaybeTake<T>(this IQueryable<T> source, IPage page)
        {
            if(page != null)
            {
                return source.Skip(page.Offset).Take(page.Size);
            }

            return source;
        }

        #region Filtering
        internal static IQueryable<T> MaybeWhere<T>(this IQueryable<T> source, ISpecification<T> specification)
        {
            if(specification != null)
            {
                return source.Where(specification.Expression);
            }

            return source;
        }

        #endregion

        #region Ordering
        internal static IQueryable<T> MaybeSort<T>(this IQueryable<T> source, IEnumerable<OrderCreteria<T>> orderCreterias)
        {
            if(orderCreterias != null && orderCreterias.Any())
            {
                foreach(var orderCreteria in orderCreterias)
                {
                    source = source.MaybeSort(orderCreteria);
                }
            }

            return source;
        }

        internal static IQueryable<T> MaybeSort<T>(this IQueryable<T> source, OrderCreteria<T> orderCreteria)
        {
            if (source.IsOrdered()) return (source as IOrderedQueryable<T>).MaybeSort(orderCreteria);

            if (orderCreteria != null)
            {
                return orderCreteria.Direction == OrderDirection.DESC ? source.OrderByMemberDescending(orderCreteria.SortKey) : source.OrderByMember(orderCreteria.SortKey);
            }

            return source;
        }

        internal static bool IsOrdered<T>(this IQueryable<T> queryable)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException("queryable");
            }

            return queryable.Expression.Type == typeof(IOrderedQueryable<T>);
        }

        internal static IOrderedQueryable<T> MaybeSort<T>(this IOrderedQueryable<T> source, OrderCreteria<T> orderCreteria)
        {
            if (orderCreteria != null)
            {
                return orderCreteria.Direction == OrderDirection.DESC ? source.ThenByMemberDescending(orderCreteria.SortKey) : source.ThenByMember(orderCreteria.SortKey);
            }

            return source;
        }

        internal static IOrderedQueryable<T> OrderByMember<T>(this IQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "OrderBy");
        }

        internal static IOrderedQueryable<T> OrderByMemberDescending<T>(this IQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "OrderByDescending");
        }

        internal static IOrderedQueryable<T> ThenByMember<T>(this IOrderedQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "ThenBy");
        }

        internal static IOrderedQueryable<T> ThenByMemberDescending<T>(this IOrderedQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "ThenByDescending");
        }

        private static IOrderedQueryable<T> OrderByMemberUsing<T>(this IQueryable<T> source, string memberPath, string method)
        {
            var rootType = typeof(T);
            var parameter = Expression.Parameter(rootType, "x");
            Expression body = parameter;
            var members = memberPath.Split('.');
            var parentType = rootType;
            foreach (var member in members)
            {
                var prop = parentType.GetTypeInfo().IsInterface ? parentType.GetInerfaceProperty(member) : parentType.GetProperty(member);
                body = Expression.Property(body, prop);
                parentType = prop.PropertyType;
            }

            var methodCall = Expression.Call(typeof(Queryable), method,
                new Type[] { source.ElementType, body.Type },
                source.Expression, Expression.Quote(Expression.Lambda(body, parameter)));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery(methodCall);
        }

        #endregion
    }
}

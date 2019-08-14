﻿namespace CqrsEase.EntityFrameworkCore
{
    #region Using
    using CqrsEase.Common;
    using CqrsEase.Common.Queries;
    using CqrsEase.Common.Queries.FetchStrategies;
    using CqrsEase.Common.Queries.Pagination;
    using CqrsEase.Common.Queries.Sorting;
    using Microsoft.EntityFrameworkCore;
    using NSpecifications;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    #endregion

    public static class IQueriableExtensions
    {
        public static IQueryable<T> ApplyFetchStrategy<T>(this IQueryable<T> source, IFetchStrategy<T> fetchStrategy)
        {
            if (fetchStrategy != null && !(fetchStrategy is FetchAllStrategy<T>))
            {
                return source.SelectOnly<T>(fetchStrategy.FetchedPaths);
            }

            return source;
        }

        public static IQueryable<T> ApplyFetchStrategy<T>(this IQueryable<T> source, IFetchStrategy<T> fetchStrategy, DbContext dbContext)
            where T : class
        {
            if (fetchStrategy != null)
            {
                var entityType = dbContext.Model.FindEntityType(typeof(T));
                if(entityType != null)
                {
                    var pathsToInclude = entityType.GetNavigations().Where(p => fetchStrategy.FetchedPaths.Contains(p.Name));
                    foreach (var path in pathsToInclude)
                    {
                        source = source.Include(path.Name);
                    }
                }

                return source.ApplyFetchStrategy(fetchStrategy);
            }

            return source;
        }



        public static IQueryable<T> SelectOnly<T>(this IQueryable<T> queryable, IEnumerable<string> paths)
        {
            var projectionSelector = BuildSelector<T>(paths);
            return queryable.Select(projectionSelector).AsQueryable();
        }

        private static Func<T, T> BuildSelector<T>(IEnumerable<string> paths)
        {
            var type = typeof(T);
            var lamdaParameter = Expression.Parameter(typeof(T), "x");
            var xNew = Expression.New(type);
            var bindings = paths.Select(path => type.GetProperty(path)).Where(prop => prop.SetMethod != null).Select(prop =>
            {
                var getPropertyExpression = Expression.Property(lamdaParameter, prop);
                // set value "Field1 = x.Field1"
                return Expression.Bind(prop, getPropertyExpression);
            });

            var xInit = Expression.MemberInit(xNew, bindings);
            // expression "x => new T { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<T, T>>(xInit, lamdaParameter);
            return lambda.Compile();
        }

        public static IQueryable<T> MaybeTake<T>(this IQueryable<T> source, IPage page)
        {
            if(page != null)
            {
                return source.Skip(page.Offset).Take(page.Size);
            }

            return source;
        }

        #region Filtering
        public static IQueryable<T> MaybeWhere<T>(this IQueryable<T> source, ISpecification<T> specification)
        {
            if(specification != null)
            {
                return source.Where(specification.Expression);
            }

            return source;
        }

        #endregion

        #region Ordering
        public static IQueryable<T> MaybeSort<T>(this IQueryable<T> source, IEnumerable<OrderCreteria<T>> orderCreterias)
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

        public static IQueryable<T> MaybeSort<T>(this IQueryable<T> source, OrderCreteria<T> orderCreteria)
        {
            if (source.IsOrdered()) return (source as IOrderedQueryable<T>).MaybeSort(orderCreteria);

            if (orderCreteria != null)
            {
                return orderCreteria.Direction == OrderDirection.DESC ? source.OrderByMemberDescending(orderCreteria.SortKey) : source.OrderByMember(orderCreteria.SortKey);
            }

            return source;
        }

        public static bool IsOrdered<T>(this IQueryable<T> queryable)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException("queryable");
            }

            return queryable.Expression.Type == typeof(IOrderedQueryable<T>);
        }

        public static IOrderedQueryable<T> MaybeSort<T>(this IOrderedQueryable<T> source, OrderCreteria<T> orderCreteria)
        {
            if (orderCreteria != null)
            {
                return orderCreteria.Direction == OrderDirection.DESC ? source.ThenByMemberDescending(orderCreteria.SortKey) : source.ThenByMember(orderCreteria.SortKey);
            }

            return source;
        }

        public static IOrderedQueryable<T> OrderByMember<T>(this IQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByMemberDescending<T>(this IQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenByMember<T>(this IOrderedQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "ThenBy");
        }

        public static IOrderedQueryable<T> ThenByMemberDescending<T>(this IOrderedQueryable<T> source, string memberPath)
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

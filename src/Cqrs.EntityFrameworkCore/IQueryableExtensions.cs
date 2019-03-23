namespace Cqrs.EntityFrameworkCore
{
    using Cqrs.Common.Queries;
    #region Using
    using Cqrs.Common.Queries.Sorting;
    using NSpecifications;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    #endregion

    internal static class IQueriableExtensions
    {
        internal static IQueryable<T> ApplyFetchStrategy<T>(this IQueryable<T> source, IFetchStrategy<T> fetchStrategy)
        {
            if (fetchStrategy == null) throw new ArgumentNullException("fetchStrategy");
            IProjector projector = null;
            return projector.ProjectOnly<T, T>(source, fetchStrategy.FetchedPaths);
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
            if (source is IOrderedQueryable<T> orderedSource) return orderedSource.MaybeSort(orderCreteria);

            if (orderCreteria != null)
            {
                return orderCreteria.Direction == OrderDirection.DESC ? source.OrderByMemberDescending(orderCreteria.SortKey) : source.OrderByMember(orderCreteria.SortKey);
            }

            return source;
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
                source.Expression,, Expression.Quote(Expression.Lambda(body, parameter)));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery(methodCall);
        }

        #endregion
    }
}

namespace Cqrs.JsonApi
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Common.Queries.Sorting;
    using NSpecifications;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    #endregion

    internal static class JsonApiExtensions
    {
        internal static string ToJsonApiInclude<TResource>(this IFetchStrategy<TResource> fetchStrategy) where TResource : IResource
        {
            var resouceBaseType = typeof(IResource).GetTypeInfo();
            var rootResourceProperties = typeof(TResource).GetTypeInfo().DeclaredProperties;
            var relatedResourcesProperies = rootResourceProperties.Where(p => resouceBaseType.IsAssignableFrom(p.PropertyType.GetTypeInfo()));
            
            if (relatedResourcesProperies != null && relatedResourcesProperies.Any())
            {

                var relatedResourcesToInlude = fetchStrategy.FetchedPaths.Where(path => relatedResourcesProperies.Any(prop => path.Contains(prop.Name)));
                var includedResourcesString = string.Join(",", relatedResourcesToInlude);
                return $"include={includedResourcesString}";
            }

            return string.Empty;
        }

        internal static string ToJsonApiSparseFields<TResource>(this IFetchStrategy<TResource> fetchStrategy) where TResource : IResource, new()
        {
            var resouceBaseType = typeof(IResource).GetTypeInfo();
            var resourceType = Activator.CreateInstance<TResource>();
            var relatedResourceSparseFields = fetchStrategy.FetchedPaths.Where(path => path.Contains("."));
            var rootResourceSparseFields = fetchStrategy.FetchedPaths.Except(relatedResourceSparseFields);

            string rootResourceSparseFieldsString = string.Join(",", rootResourceSparseFields);
            string result = $"fields[{resourceType.Type}]={rootResourceSparseFieldsString}";

            if(relatedResourceSparseFields.Any())
            {
                //TODO: finish fileds to related reources

                string relatedResourceSparseFieldsString = string.Join(",", relatedResourceSparseFields);
                result += $"fields[{resourceType.Type}]={rootResourceSparseFieldsString}";
            }

            return result;
        }
        

        internal static string ToJsonApiSorting<TResource>(this IEnumerable<OrderCreteria<TResource>> orderCreterias) where TResource : IResource
        {
            string sortFields = string.Join(",", orderCreterias.Select(c => c.ToJsonApiParameter()));
            return $"sort={sortFields}";
        }

        internal static string ToJsonApiParameter<TResource>(this OrderCreteria<TResource> orderCreteria) where TResource : IResource
        {
            return $"{orderCreteria.Direction}{orderCreteria.SortKey}";
        }

        internal static string ToJsonApiPaging(this IPage page)
        {
            return $"page[size]={page.Size}&page[number]={page.Number}";
        }

        #region Filtering

        internal static string ToJsonApiFilter<TResource>(this ISpecification<TResource> specification) where TResource : IResource
        {
            var lambda = specification.Expression;
            return BuildFilterParams(lambda.Body);
        }

        private static string BuildFilterParams(Expression expression)
        {
            string filterParams = string.Empty;
            if (expression.NodeType == ExpressionType.Equal)
            {
                BinaryExpression eq = (BinaryExpression)expression;
                MemberExpression proteprtySelector = (MemberExpression)eq.Left;
                string propertyName = proteprtySelector.Member.Name;
                object value = Evaluate(eq.Right);
                filterParams = $"filter[{propertyName}]={value}";
            }
            else if (expression is BinaryExpression binaryExpression)
            {
                filterParams = string.Join("&", BuildFilterParams(binaryExpression.Left), BuildFilterParams(binaryExpression.Right));
            }

            return filterParams;
        }

        private static object Evaluate(Expression e)
        {
            if (e.NodeType == ExpressionType.Constant)
                return ((ConstantExpression)e).Value;
            return Expression.Lambda(e).Compile().DynamicInvoke();
        }

        #endregion
    }
}

namespace CqrsEase.JsonApi.Web.Request
{
    #region Using
    using CqrsEase.Common;
    using CqrsEase.Common.Queries.FetchStrategies;
    using CqrsEase.Common.Queries.Sorting;
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
                var relatedResourcesToInlude = fetchStrategy.FetchedPaths.Where(path => relatedResourcesProperies.Any(prop => path.Contains(prop.Name))).Select(f => f.ToCamelCase());
                var includedResourcesString = string.Join(",", relatedResourcesToInlude);
                return includedResourcesString;
            }

            return string.Empty;
        }

        internal static IDictionary<string, string> ToJsonApiSparseFields<TResource>(this IFetchStrategy<TResource> fetchStrategy) where TResource : IResource, new()
        {
            var result = new Dictionary<string, string>();
            var resourceType = Activator.CreateInstance<TResource>();
            var relatedResourceSparseFields = fetchStrategy.FetchedPaths.Where(path => path.Contains("."));
            var rootResourceSparseFields = fetchStrategy.FetchedPaths.Except(relatedResourceSparseFields).Select(f => f.ToCamelCase());

            string rootResourceSparseFieldsString = string.Join(",", rootResourceSparseFields);
            result.Add(resourceType.Type, rootResourceSparseFieldsString);

            if (relatedResourceSparseFields.Any())
            {
                //TODO: finish fileds to related reources
            }

            return result;
        }

        internal static string ToJsonApiParameter<TResource>(this OrderCreteria<TResource> orderCreteria) where TResource : IResource
        {
            string direction = orderCreteria.Direction == OrderDirection.DESC ? "-" : string.Empty;
            return $"{direction}{orderCreteria.SortKey.ToCamelCase()}";
        }

        #region Filtering

        internal static IDictionary<string, string> ToJsonApiFilter<TResource>(this ISpecification<TResource> specification) where TResource : IResource
        {
            var lambda = specification.Expression;
            var result = new Dictionary<string, string>();
            BuildFilterParams(lambda.Body, ref result);
            return result;
        }

        private static void BuildFilterParams(Expression expression, ref Dictionary<string, string> result)
        {
            if (expression.NodeType == ExpressionType.Equal)
            {
                BinaryExpression eq = (BinaryExpression)expression;
                MemberExpression proteprtySelector = (MemberExpression)eq.Left;
                string propertyName = proteprtySelector.Member.Name;
                object value = Evaluate(eq.Right);
                result.Add(propertyName.ToCamelCase(), value.ToString().ToCamelCase());
            }
            else if (expression is BinaryExpression binaryExpression)
            {
                BuildFilterParams(binaryExpression.Left, ref result);
                BuildFilterParams(binaryExpression.Right, ref result);
            }
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

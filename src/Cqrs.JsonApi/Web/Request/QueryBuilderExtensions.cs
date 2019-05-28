namespace Cqrs.JsonApi.Web.Request
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Common.Queries.Sorting;
    using Microsoft.AspNetCore.Http.Extensions;
    using NSpecifications;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    internal static class QueryBuilderExtensions
    {
        internal static QueryBuilder AddSparseFieldsets<TResource>(this QueryBuilder queryBuilder, IFetchStrategy<TResource> fetchStrategy) where TResource : IResource, new()
        {
            var resourcesSparseFields = fetchStrategy.ToJsonApiSparseFields();
            foreach (var resourceSparseFields in resourcesSparseFields)
            {
                var paramName = string.Format(JsonApiQueryParams.SparseFieldsetsTemplate, resourceSparseFields.Key);
                var paramValue = resourceSparseFields.Value;
                queryBuilder.Add(paramName, paramValue);
            }

            return queryBuilder;
        }

        internal static QueryBuilder AddIncludedResources<TResource>(this QueryBuilder queryBuilder, IFetchStrategy<TResource> fetchStrategy) where TResource : IResource
        {
            queryBuilder.Add(JsonApiQueryParams.Include, fetchStrategy.ToJsonApiInclude());
            return queryBuilder;
        }

        internal static QueryBuilder AddFilter<TResource>(this QueryBuilder queryBuilder, ISpecification<TResource> specification) where TResource : IResource
        {
            var resourceFieldsFilters = specification.ToJsonApiFilter();
            foreach (var resourceFieldsFilter in resourceFieldsFilters)
            {
                var paramName = string.Format(JsonApiQueryParams.FieldFilterTemplate, resourceFieldsFilter.Key);
                var paramValue = resourceFieldsFilter.Value;
                queryBuilder.Add(paramName, paramValue);
            }

            return queryBuilder;
        }

        internal static QueryBuilder AddPaging(this QueryBuilder queryBuilder, IPage page)
        {
            queryBuilder.Add(JsonApiQueryParams.PageSize, page.Size.ToString());
            queryBuilder.Add(JsonApiQueryParams.PageNumber, page.Number.ToString());
            return queryBuilder;
        }

        internal static QueryBuilder AddSorting<TResource>(this QueryBuilder queryBuilder, IEnumerable<OrderCreteria<TResource>> orderCreterias) where TResource : IResource
        {
            string sortFields = string.Join(",", orderCreterias.Select(c => c.ToJsonApiParameter()));
            queryBuilder.Add(JsonApiQueryParams.Sorting, sortFields);
            return queryBuilder;
        }
    }
}

namespace CqrsEase.JsonApi.Web.Request
{
    using CqrsEase.Common;
    #region Using
    using CqrsEase.Common.Queries;
    using CqrsEase.Common.Queries.FetchStrategies;
    using CqrsEase.Common.Queries.Pagination;
    using CqrsEase.Common.Queries.Sorting;
    using Microsoft.AspNetCore.Http.Extensions;
    using NSpecifications;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    internal static class QueryBuilderExtensions
    {
        internal static QueryBuilder MaybeAddSparseFieldsets<TResource>(this QueryBuilder queryBuilder, IFetchStrategy<TResource> fetchStrategy) where TResource : IResource, new()
        {
            if (fetchStrategy != null && !(fetchStrategy is FetchAllStrategy<TResource>))
            {
                var resourcesSparseFields = fetchStrategy.ToJsonApiSparseFields();
                foreach (var resourceSparseFields in resourcesSparseFields)
                {
                    var paramName = string.Format(JsonApiQueryParams.SparseFieldsetsTemplate, resourceSparseFields.Key);
                    var paramValue = resourceSparseFields.Value;
                    queryBuilder.Add(paramName, paramValue);
                }
            }

            return queryBuilder;
        }

        internal static QueryBuilder MaybeAddIncludedResources<TResource>(this QueryBuilder queryBuilder, IFetchStrategy<TResource> fetchStrategy) where TResource : IResource
        {
            if (fetchStrategy != null)
            {
                queryBuilder.Add(JsonApiQueryParams.Include, fetchStrategy.ToJsonApiInclude());
            }

            return queryBuilder;
        }

        internal static QueryBuilder MaybeAddFilter<TResource>(this QueryBuilder queryBuilder, ISpecification<TResource> specification) where TResource : IResource
        {
            if (specification != null)
            {
                var resourceFieldsFilters = specification.ToJsonApiFilter();
                foreach (var resourceFieldsFilter in resourceFieldsFilters)
                {
                    var paramName = string.Format(JsonApiQueryParams.FieldFilterTemplate, resourceFieldsFilter.Key);
                    var paramValue = resourceFieldsFilter.Value;
                    queryBuilder.Add(paramName, paramValue);
                }
            }

            return queryBuilder;
        }

        internal static QueryBuilder MaybeAddPaging(this QueryBuilder queryBuilder, IPage page)
        {
            if (page != null)
            {
                queryBuilder.Add(JsonApiQueryParams.PageSize, page.Size.ToString());
                queryBuilder.Add(JsonApiQueryParams.PageNumber, page.Number.ToString());
            }

            return queryBuilder;
        }

        internal static QueryBuilder MaybeAddSorting<TResource>(this QueryBuilder queryBuilder, IEnumerable<OrderCreteria<TResource>> orderCreterias) where TResource : IResource
        {
            if (orderCreterias != null)
            {
                string sortFields = string.Join(",", orderCreterias.Select(c => c.ToJsonApiParameter()));
                queryBuilder.Add(JsonApiQueryParams.Sorting, sortFields);
            }

            return queryBuilder;
        }
    }
}

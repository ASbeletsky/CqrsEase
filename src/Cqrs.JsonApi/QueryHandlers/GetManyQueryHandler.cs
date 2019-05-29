namespace Cqrs.JsonApi.QueryHandlers
{
    #region 
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Core.Abstractions;
    using Cqrs.JsonApi.Web;
    using Cqrs.JsonApi.Web.Request;
    using Microsoft.AspNetCore.Http.Extensions;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    #endregion

    public class GetManyQueryHandler<TResource>
        : IQueryHandlerAsync<GetManyQuery<TResource>, IEnumerable<TResource>>
        where TResource : IResource, new()
    {
        public GetManyQueryHandler(string baseUrl)
        {
            ResourceEndpoint = new JsonApiClient(baseUrl).For<IJsonApiEndpoint<TResource>>();
        }

        public IJsonApiEndpoint<TResource> ResourceEndpoint { get; private set; }

        protected string BuildQueryString(GetManyQuery<TResource> query)
        {
            var queryString = new QueryBuilder()
                .MaybeAddIncludedResources(query.FetchStrategy)
                .MaybeAddFilter(query.Specification)
                .MaybeAddPaging(query.Pagination)
                .MaybeAddSorting(query.Sorting)
                .MaybeAddSparseFieldsets(query.FetchStrategy)
                .ToQueryString();

            return queryString.Value;
        }

        public async Task<IEnumerable<TResource>> RequestAsync(GetManyQuery<TResource> query)
        {
            var queryString = BuildQueryString(query);
            return await ResourceEndpoint.Get(queryString);
        }
    }
}

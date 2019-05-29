namespace Cqrs.JsonApi.QueryHandlers
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Core.Abstractions;
    using Cqrs.JsonApi.Web;
    using Cqrs.JsonApi.Web.Request;
    using Microsoft.AspNetCore.Http.Extensions;
    using RestEase;
    using System.Linq;
    using System.Threading.Tasks;
    #endregion

    public class GetFirstQueryHandler<TResource>
        : IQueryHandlerAsync<GetFirstQuery<TResource>, TResource>
        where TResource : IResource, new()
    {
        public GetFirstQueryHandler(string baseUrl)
        {
            ResourceEndpoint = new JsonApiClient(baseUrl).For<IJsonApiEndpoint<TResource>>();
        }

        public IJsonApiEndpoint<TResource> ResourceEndpoint { get; private set; }

        public async Task<TResource> RequestAsync(GetFirstQuery<TResource> query)
        {
            var takeOnePaging = new Page(pageNumber: 1, pageSize: 1);
            var queryString = new QueryBuilder()
                .AddIncludedResources(query.FetchStrategy)
                .AddSparseFieldsets(query.FetchStrategy)
                .AddFilter(query.Specification)
                .AddPaging(takeOnePaging)
                .AddSorting(query.Sorting)
                .ToQueryString();

            var result = await RestClient.Get(queryString.Value);
            return result.FirstOrDefault();
        }
    }

}

using Cqrs.Common.Queries;
using Cqrs.Core.Abstractions;
using RestEase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs.JsonApi.QueryHandlers
{
    internal class GetFirstQueryHandler<TResource>
        : IQueryHandlerAsync<GetFirstQuery<TResource>, TResource>
        where TResource : IResource
    {
        public GetFirstQueryHandler(string baseUrl)
        {
            RestClient = new RestClient(baseUrl).For<JsonApiBased<TResource>>();
        }

        public JsonApiBased<TResource> RestClient { get; private set; }

        public Task<TResource> RequestAsync(GetFirstQuery<TResource> query)
        {
            throw new NotImplementedException();
        }
    }

}

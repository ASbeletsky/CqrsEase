using Newtonsoft.Json;
using RestEase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cqrs.JsonApi
{
    public interface IRestApiBased<T, TKey>
    {
        Task<T> Post(T paylod);

        Task<T> Get(TKey id);

        Task<IEnumerable<T>> Get(string queryString);

        Task PUT(TKey id, T payload);

        Task Patch(TKey id, T payload);

        Task Delete(TKey key);
    }

    public abstract class RestApiBased<T, TKey> : IRestApiBased<T, TKey>
    {
        protected RestClient RestClient { get; }
        public string BaseUrl { get; }

        protected RestApiBased(string baseUrl, JsonSerializerSettings jsonSerializerSettings = null)
        {
            RestClient = new RestClient(baseUrl);
            if(jsonSerializerSettings != null)
            {
                RestClient.JsonSerializerSettings = jsonSerializerSettings;
            }

            BaseUrl = baseUrl;
        }

        [Post()]
        public abstract Task<T> Post([Body] T paylod);

        [Get("{id}")]
        public abstract Task<T> Get([Path] TKey id);

        [Get()]
        public abstract Task<IEnumerable<T>> Get([RawQueryString] string queryString);

        [Put("{id}")]
        public abstract Task PUT([Path]TKey id, [Body] T payload);

        [Patch("{id}")]
        public abstract Task Patch([Path]TKey id, [Body] T payload);

        [Delete("{id}")]
        public abstract Task Delete([Path] TKey key);  
    }
}

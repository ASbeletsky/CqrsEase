namespace Cqrs.JsonApi.Web
{
    #region Using
    using Newtonsoft.Json;
    using RestEase;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    #endregion

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

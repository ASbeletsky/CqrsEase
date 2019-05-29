namespace Cqrs.JsonApi.Web
{
    using RestEase;
    #region Using
    using System.Collections.Generic;
    using System.Threading.Tasks;
    #endregion

    public interface IRestApiEndpoint<T, TKey>
    {
        [Post()]
        Task<T> Post([Body] T paylod);

        [Get("{id}")]
        Task<T> Get([Path] TKey id);

        [Get()]
        Task<IEnumerable<T>> Get([RawQueryString] string queryString);

        [Put("{id}")]
        Task Put([Path]TKey id, [Body] T payload);

        [Patch("{id}")]
        Task Patch([Path]TKey id, [Body] T payload);

        [Delete("{id}")]
        Task Delete([Path] TKey id);
    }
}

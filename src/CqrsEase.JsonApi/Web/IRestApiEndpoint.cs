namespace CqrsEase.JsonApi.Web
{
    using RestEase;
    #region Using
    using System.Collections.Generic;
    using System.Threading.Tasks;
    #endregion

    public interface IRestApiEndpoint<T, TKey>
    {
        [Get()]
        Task<IEnumerable<T>> Get([RawQueryString] string queryString);

        [Post()]
        Task<T> Post([Body] T paylod);

        [Put("{id}")]
        Task Put([Path]TKey id, [Body] T payload);

        [Patch("{id}")]
        Task Patch([Path]TKey id, [Body] T payload);

        [Delete("{id}")]
        Task Delete([Path] TKey id);
    }
}

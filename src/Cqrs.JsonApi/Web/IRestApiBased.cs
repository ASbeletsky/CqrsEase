namespace Cqrs.JsonApi.Web
{
    #region Using
    using System.Collections.Generic;
    using System.Threading.Tasks;
    #endregion

    public interface IRestApiBased<T, TKey>
    {
        Task<T> Post(T paylod);

        Task<T> Get(TKey id);

        Task<IEnumerable<T>> Get(string queryString);

        Task PUT(TKey id, T payload);

        Task Patch(TKey id, T payload);

        Task Delete(TKey key);
    }
}

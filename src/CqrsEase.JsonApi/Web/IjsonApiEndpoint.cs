namespace CqrsEase.JsonApi.Web
{
    #region Using
    using JsonApiSerializer.JsonApi;
    using RestEase;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    #endregion

    public interface IJsonApiEndpoint<TResource> : IRestApiEndpoint<TResource, string>
         where TResource : IResource
    {
        [Get("{id}")]
        Task<DocumentRoot<TResource>> Find([Path] string id);

        [Get()]
        new Task<DocumentRoot<IEnumerable<TResource>>> Get([RawQueryString] string queryString);

        [Post()]
        new Task<DocumentRoot<TResource>> Post([Body] TResource paylod);        
        
        [Patch("{id}")]
        new Task<DocumentRoot<TResource>> Patch([Path]string id, [Body] TResource payload);

        [Delete("{id}")]
        new Task<DocumentRoot<TResource>> Delete([Path] string id);
    }
}

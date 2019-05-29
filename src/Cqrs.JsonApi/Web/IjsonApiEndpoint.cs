namespace Cqrs.JsonApi.Web
{
    public interface IJsonApiEndpoint<TResource> : IRestApiEndpoint<TResource, string>
         where TResource : IResource
    {
    }
}

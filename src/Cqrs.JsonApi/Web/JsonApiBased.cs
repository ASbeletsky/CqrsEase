namespace Cqrs.JsonApi.Web
{
    using JsonApiSerializer;

    public abstract class JsonApiBased<TResource> : RestApiBased<TResource, string>
        where TResource : IResource
    {
        protected JsonApiBased(string baseUrl) : base(baseUrl, new JsonApiSerializerSettings())
        {
        }
    }
}

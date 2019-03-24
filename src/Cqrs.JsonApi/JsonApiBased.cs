using JsonApiSerializer;
using RestEase;
using System.Threading.Tasks;

namespace Cqrs.JsonApi
{
    public abstract class JsonApiBased<TResource> : RestApiBased<TResource, string>
        where TResource : IResource
    {
        protected JsonApiBased(string baseUrl) : base(baseUrl, new JsonApiSerializerSettings())
        {
        }
    }
}

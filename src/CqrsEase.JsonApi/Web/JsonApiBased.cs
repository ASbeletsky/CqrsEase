namespace CqrsEase.JsonApi.Web
{
    using JsonApiSerializer;
    using RestEase;

    public class JsonApiClient : RestClient
    {
        public JsonApiClient(string baseUrl) : base(baseUrl)
        {
            this.JsonSerializerSettings = new JsonApiSerializerSettings();
        }
    }
}

using CqrsEase.JsonApi;

namespace CqrsEase.Tests.Model
{
    public class AuthorDto : IResource
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Type => "author";
    }
}

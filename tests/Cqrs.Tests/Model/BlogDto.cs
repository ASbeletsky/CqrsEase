using Cqrs.JsonApi;
using System.Collections.Generic;

namespace Cqrs.Tests.Model
{
    public class BlogDto : IResource
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public AuthorDto Author { get; set; }
        public IEnumerable<string> Comments { get; set; }

        public string Type => "blog";
    }
}

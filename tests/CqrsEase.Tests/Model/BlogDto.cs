using CqrsEase.JsonApi;
using System.Collections.Generic;

namespace CqrsEase.Tests.Model
{
    public class BlogDto : IResource
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public AuthorDto Author { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }

        public string Type => "blog";
    }
}

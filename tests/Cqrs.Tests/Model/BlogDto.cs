using System.Collections.Generic;

namespace Cqrs.Tests.Model
{
    public class BlogDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Comments { get; set; }
    }
}

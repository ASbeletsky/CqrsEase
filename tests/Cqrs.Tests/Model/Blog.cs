using System.Collections.Generic;

namespace Cqrs.Tests.Model
{
    internal class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}

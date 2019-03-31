using System.Collections.Generic;

namespace Cqrs.Tests.Model
{
    internal class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}
